using com.manager.front.helpers;
using com.manager.front.model.book;
using com.manager.front.model.book.ports.outputs;
using com.manager.front.service.factory.exception;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace com.manager.front.Components.Pages.book
{
	public partial class AddBookPage : ComponentBase
	{
		public Book book { get; set; } = new Book();
		private List<Book> existingBooks = new List<Book>();
		private string errorMessage;
		private bool showSuccessMessage = false;
		private string existingBooksMessage = "";
		private bool isTitleInvalid = false;
		private bool isImageValid = true;

		private readonly IBookService service;
		private readonly NavigationManager navigation;
		private readonly IJSRuntime runtime;
		private readonly IWebHostEnvironment env;

		protected override async Task OnInitializedAsync()
		{
			try
			{
				existingBooks = await service.GetAll();
			}
			catch (CustomHttpRequestException ex)
			{
				errorMessage = ex.Message;
			}

		}

		public AddBookPage(IBookService service, NavigationManager navigation, IJSRuntime runtime, IWebHostEnvironment env)
		{
			this.service = service;
			this.navigation = navigation;
			this.runtime = runtime;
			this.env = env;
		}

		private async Task AddBook()
		{
			try
			{
				await service.Create(book);
				await runtime.ToastrSuccess($"libro {book.Title} creado correctamente");
				book = new Book();

			}
			catch (CustomHttpRequestException ex)
			{
				errorMessage = ex.Message;
				await runtime.ToastrError($"error al crear el libro {book.Title}\nError: {errorMessage}");
			}

		}


		private void AddAndExit()
		{
			try
			{
				Thread.Sleep(200);
				navigation.NavigateTo("/library");

			}
			catch (CustomHttpRequestException ex)
			{
				errorMessage = ex.Message;
			}

		}

		private void Cancel()
		{
			book = new Book();
			showSuccessMessage = false;
			navigation.NavigateTo("/library");

		}

		private void CheckTitle(ChangeEventArgs e)
		{
			var inputTitle = e.Value?.ToString();
			if (!string.IsNullOrEmpty(inputTitle) && existingBooks.Any(b => b.Title.Equals(inputTitle, StringComparison.OrdinalIgnoreCase)))
			{
				existingBooksMessage = "Ya existe un libro con este título.";
				isTitleInvalid = true;
			}
			else
			{
				existingBooksMessage = string.Empty;
				isTitleInvalid = false;
			}

		}

		private async Task HandleImageUpload(InputFileChangeEventArgs e)
		{
			var (isValid, message, imagePath) = await FileUploadHelper.ValidUploadImageAsync(e, env);
			errorMessage = message;
			isImageValid = isValid;

			if (isValid)
			{
				book.ImagePath = imagePath;
				Console.WriteLine(book.ImagePath);
			}
		}

		private void DiscardImage()
		{
			FileUploadHelper.DiscardImage(book.ImagePath, env);
			book.ImagePath = null;
			isImageValid = true;
			errorMessage = string.Empty;
		}


	}
}
