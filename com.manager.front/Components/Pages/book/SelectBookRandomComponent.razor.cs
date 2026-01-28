using com.manager.front.model.book;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace com.manager.front.Components.Pages.book
{
	public partial class SelectBookRandomComponent : ComponentBase
	{
		[Parameter] public bool ShowRouletteModal { get; set; }
		[Parameter] public List<Book> UnreadBooks { get; set; } = new();
		[Parameter] public EventCallback<bool> OnClose { get; set; }

		private Book SelectedBook;

		private readonly IJSRuntime runtime;

		public SelectBookRandomComponent(IJSRuntime runtime)
		{
			this.runtime = runtime;
		}

		private async Task CloseRouletteModal()
		{
			await OnClose.InvokeAsync(false);
		}

		private async Task SpinWheel()
		{
			if (UnreadBooks.Any())
			{
				await runtime.InvokeVoidAsync("spinWheel");
				var random = new Random();
				SelectedBook = UnreadBooks[random.Next(UnreadBooks.Count)];
				await Task.Delay(2000);

			}
		}
	}
}
