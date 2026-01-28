using com.manager.front.model.book;
using com.manager.front.service.factory;

namespace com.manager.front.service.bookService.repository
{
	public class BookRepository(HttpClient httpClient, string baseUrl) : BaseRepository<Book>(httpClient), IBookRepository
	{

		private readonly string _baseUrl = baseUrl;

		protected override string GetBaseUrl() => _baseUrl;

	}
}
