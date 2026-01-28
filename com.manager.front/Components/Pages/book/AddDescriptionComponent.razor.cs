using com.manager.front.model.book;
using Microsoft.AspNetCore.Components;

namespace com.manager.front.Components.Pages.book
{
	public partial class AddDescriptionComponent : ComponentBase
	{
		[Parameter] public bool ShowModal { get; set; }
		[Parameter] public Book Book { get; set; } = new Book();
		[Parameter] public EventCallback<bool> OnClose { get; set; }
		[Parameter] public EventCallback<Book> OnSave { get; set; }

		private async Task SaveChanges()
		{
			await OnSave.InvokeAsync(Book);
			await CloseModal();
		}
		private async Task CloseModal()
		{
			await OnClose.InvokeAsync(false);
		}
	}
}
