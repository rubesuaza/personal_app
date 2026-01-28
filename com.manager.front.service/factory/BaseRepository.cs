
using com.manager.front.service.factory.dtos;
using com.manager.front.service.factory.exception;
using Polly;
using Polly.Retry;
using System.Text;
using System.Text.Json;

namespace com.manager.front.service.factory
{
	public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
	{

		private readonly HttpClient _httpClient;
		private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;

		protected BaseRepository(HttpClient httpClient)
		{
			_httpClient = httpClient;

			_retryPolicy = Policy
			.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
			.Or<TimeoutException>()
			.WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
		}

		protected abstract string GetBaseUrl();

		public async Task<ObjectResponse<IEnumerable<T>>> GetAllAsync(string endpoint)
		{
			try
			{
				var response = await ExecuteRequestAsync(() => _httpClient.GetAsync($"{GetBaseUrl()}/{endpoint}"));
				var content = await response.Content.ReadAsStringAsync();
				return JsonSerializer.Deserialize<ObjectResponse<IEnumerable<T>>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
			}
			catch (HttpRequestException ex)
			{
				throw new CustomHttpRequestException("se presento un error en el servicio", ex);
			}
		}

		public async Task<ObjectResponse<T>> GetByIdAsync(string endpoint)
		{
			try
			{
				var response = await ExecuteRequestAsync(() => _httpClient.GetAsync($"{GetBaseUrl()}/{endpoint}"));
				response.EnsureSuccessStatusCode();
				var content = await response.Content.ReadAsStringAsync();
				return JsonSerializer.Deserialize<ObjectResponse<T>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
			}
			catch (HttpRequestException ex)
			{
				throw new CustomHttpRequestException("se presento un error en el servicio", ex);
			}
		}

		public async Task<ObjectResponse<T>> AddAsync(T entity, string endpoint)
		{
			try
			{
				var content = new StringContent(JsonSerializer.Serialize(entity), Encoding.UTF8, "application/json");
				var response = await ExecuteRequestAsync(() => _httpClient.PostAsync($"{GetBaseUrl()}/{endpoint}", content));
				var responseBody = await response.Content.ReadAsStringAsync();
				return JsonSerializer.Deserialize<ObjectResponse<T>>(responseBody,
					new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
			}
			catch (HttpRequestException ex)
			{
				throw new CustomHttpRequestException("se presento un error en el servicio", ex);
			}
		}

		public async Task<ObjectResponse<T>> UpdateAsync(T entity, string endpoint)
		{
			try
			{
				var content = new StringContent(JsonSerializer.Serialize(entity), Encoding.UTF8, "application/json");
				var response = await ExecuteRequestAsync(() => _httpClient.PutAsync($"{GetBaseUrl()}/{endpoint}", content));
				var responseBody = await response.Content.ReadAsStringAsync();
				return JsonSerializer.Deserialize<ObjectResponse<T>>(responseBody,
					new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
			}
			catch (HttpRequestException ex)
			{
				throw new CustomHttpRequestException("se presento un error en el servicio", ex);
			}
		}

		public async Task DeleteAsync(int id, string endpoint)
		{
			try
			{
				var response = await ExecuteRequestAsync(() => _httpClient.DeleteAsync($"{GetBaseUrl()}/{endpoint}?id={id}"));
				response.EnsureSuccessStatusCode();
			}
			catch (HttpRequestException ex)
			{
				throw new CustomHttpRequestException("se presento un error en el servicio", ex);
			}
		}

		private async Task<HttpResponseMessage> ExecuteRequestAsync(Func<Task<HttpResponseMessage>> request)
		{
			try
			{
				return await _retryPolicy.ExecuteAsync(request);
			}
			catch (Exception ex)
			{
				throw new CustomHttpRequestException("Error al obtener el recurso", ex);
			}
		}
	}
}
