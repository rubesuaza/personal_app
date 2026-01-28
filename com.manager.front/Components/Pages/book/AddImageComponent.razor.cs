using com.manager.front.helpers;
using com.manager.front.model.book;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace com.manager.front.Components.Pages.book
{
	public partial class AddImageComponent : ComponentBase
	{
		[Parameter] public bool ShowModal { get; set; }
		[Parameter] public Book Book { get; set; } = new Book();
		[Parameter] public EventCallback<bool> OnClose { get; set; }
		[Parameter] public EventCallback<Book> OnSave { get; set; }

		private string errorMessage;
		private bool isImageValid = true;
		private string imagePathDiscard;

		private readonly IWebHostEnvironment env;

		public AddImageComponent(IWebHostEnvironment env)
		{
			this.env = env;
		}

		private async Task SaveChanges()
		{
			await OnSave.InvokeAsync(Book);
			if (!String.IsNullOrEmpty(imagePathDiscard))
			{
				FileUploadHelper.DiscardImage(imagePathDiscard, env);
				imagePathDiscard = String.Empty;
			}
			await CloseModal();
		}
		private async Task CloseModal()
		{
			if (!String.IsNullOrEmpty(imagePathDiscard))
			{
				FileUploadHelper.DiscardImage(Book.ImagePath, env);
				Book.ImagePath = imagePathDiscard;
				imagePathDiscard = string.Empty;
			}
			await OnClose.InvokeAsync(false);
		}

		private async Task HandleImageUpload(InputFileChangeEventArgs e)
		{
			var (isValid, message, imagePath) = await FileUploadHelper.ValidUploadImageAsync(e, env);
			errorMessage = message;
			isImageValid = isValid;

			if (isValid)
			{
				imagePathDiscard = Book.ImagePath;
				Book.ImagePath = imagePath;

			}
		}

		private void DiscardImage()
		{
			FileUploadHelper.DiscardImage(Book.ImagePath, env);
			Book.ImagePath = null;
			isImageValid = true;
			errorMessage = string.Empty;
		}
	}
}
