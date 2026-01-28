using com.manager.front.helpers;
using com.manager.front.model.book;
using com.manager.front.model.book.ports.outputs;
using com.manager.front.service.factory.exception;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace com.manager.front.Components.Pages.book
{
	public partial class BookDetailPage : ComponentBase
	{
		[Parameter] public int BookId { get; set; }
		private Book Book { get; set; } = new();
		private bool showImageModal = false;
		private bool showDescriptionModal = false;
		private bool showPersonalReviewModal = false;
		private bool showRaitingModal = false;

		private readonly IBookService service;
		private readonly NavigationManager navigation;
		private readonly SweetAlertService alertService;
		private readonly IBookFileService bookFileService;
		private readonly IJSRuntime runtime;
		private readonly IWebHostEnvironment env;

		public BookDetailPage(IBookService service, NavigationManager navigation,
			SweetAlertService alertService, IBookFileService bookFileService, IJSRuntime runtime,
			IWebHostEnvironment env)
		{
			this.service = service;
			this.navigation = navigation;
			this.alertService = alertService;
			this.bookFileService = bookFileService;
			this.runtime = runtime;
			this.env = env;
		}



		protected override async Task OnInitializedAsync()
		{
			Book = await service.GetById(BookId);
		}

		private void GoBack()
		{
			navigation.NavigateTo("/library");
		}

		private string GetRatingClass(int rating)
		{
			return rating switch
			{
				<= 4 => "bg-danger text-white",
				<= 7 => "bg-warning text-dark",
				<= 9 => "bg-success text-white",
				10 => "bg-primary text-white",
				_ => "bg-secondary text-white"
			};
		}

		private void OpenImageModal()
		{
			showImageModal = true;
		}
		private void OpenDescriptionModal()
		{
			showDescriptionModal = true;
		}
		private void OpenPersonalReviwModal()
		{
			showPersonalReviewModal = true;
		}
		private void OpenRaitingModal()
		{
			showRaitingModal = true;
		}


		private async Task SaveBookChanges()
		{

			if (await alertService.ShowConfirmation("Editar", $"Esta seguro de editar el libro {Book.Title}?"))
			{

				try
				{
					await service.Edit(Book);
					await alertService.ShowAlert("Editar Libro", $"Libro {Book.Title} editado correctamente", "success");
				}
				catch (CustomHttpRequestException ex)
				{
					await alertService.ShowAlert("Editar Libro", $"error al editar el libro {Book.Title}\n Error: {ex.Message}", "error");
				}
			}

			showImageModal = false;
			showDescriptionModal = false;
			showRaitingModal = false;


		}
		private void CloseImageModal(bool isClosed)
		{
			showImageModal = false;
		}
		private void CloseDescriptionModal()
		{
			showDescriptionModal = false;
		}
		private void ClosePersonalReviewModal()
		{
			showPersonalReviewModal = false;
		}
		private void CloseRaitingModal()
		{
			showRaitingModal = false;
		}

		private async Task DownloadPdf()
		{
			if (Book != null)
			{

				var imagePath = Path.Combine(env.WebRootPath, Book.ImagePath); ;
				var pdfData = bookFileService.GenerateBookDetailPdf(Book, imagePath);
				var base64 = Convert.ToBase64String(pdfData);

				// Llamar al JavaScript para descargar el archivo
				await runtime.InvokeVoidAsync("downloadFileFromBytes", $"{Book.Title}.pdf", base64);
			}
		}


	}
}
