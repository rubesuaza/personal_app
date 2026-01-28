using Microsoft.JSInterop;

namespace com.manager.front.helpers
{
	public static class IJsHelper
	{
		public static async ValueTask ToastrSuccess(this IJSRuntime jSRuntime, string message)
		{
			await jSRuntime.InvokeVoidAsync("ShowToastr", "success", message);
		}
		public static async ValueTask ToastrError(this IJSRuntime jSRuntime, string message)
		{
			await jSRuntime.InvokeVoidAsync("ShowToastr", "error", message);
		}
	}
}
