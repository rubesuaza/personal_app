using Microsoft.JSInterop;

namespace com.manager.front.helpers
{
	public class SweetAlertService
	{
		private readonly IJSRuntime runtime;

		public SweetAlertService(IJSRuntime runtime)
		{
			this.runtime = runtime;
		}

		public async Task ShowAlert(string title, string text, string icon)
		{
			await runtime.InvokeVoidAsync("SweetAlertHelper.showAlert", title, text, icon);
		}

		public async Task<bool> ShowConfirmation(string title, string text)
		{

			return await runtime.InvokeAsync<bool>("SweetAlertHelper.showConfirmation", title, text); ;

		}
	}
}
