using com.manager.front.helpers;
using com.manager.front.model.book;
using com.manager.front.model.book.ports.outputs;
using com.manager.front.service.factory.exception;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;




namespace com.manager.front.Components.Pages.book
{
	public partial class BookPage : ComponentBase
	{
		#region Properties and Fields
		public Book book { get; set; } = new Book();
		private List<Book> Books = new List<Book>();
		private List<Book> UnReadBooks = new();

		private readonly IBookService service;
		private readonly SweetAlertService alertService;
		private readonly IWebHostEnvironment env;
		private readonly IJSRuntime runtime;
		private readonly NavigationManager navigation;

		private PaginationService<Book> pagination;
		private string SearchTerm = "";
		private string SelectedStatus { get; set; } = "";
		private string SelectedInStock { get; set; } = "";
		private string errorMessage;

		private bool showEditModal = false;
		private bool showDescriptionModal = false;
		private bool showImageModal = false;
		private bool showSelectRandomBookModal = false;

		private Book selectedBook = new Book();
		private IEnumerable<Book> PaginatedBooks => pagination?.GetCurrentPageItems() ?? Enumerable.Empty<Book>();

		private Book CurrentBookInProcess => Books.FirstOrDefault(b => b.Status.Equals("en_proceso"));

		#endregion

		#region Constructor
		public BookPage(IBookService service, SweetAlertService alertService,
			IWebHostEnvironment env, IJSRuntime runtime, NavigationManager navigation)
		{
			this.service = service;
			this.alertService = alertService;
			this.env = env;
			this.runtime = runtime;
			this.navigation = navigation;
		}

		#endregion


		#region Lifecycle Methods

		protected override async Task OnInitializedAsync()
		{
			try
			{
				Books = (await service.GetAll()).OrderBy(book => book.Title).ToList();
			}
			catch (CustomHttpRequestException ex)
			{
				errorMessage = ex.Message;
			}

		}

		protected override void OnParametersSet()
		{
			if (Books != null && Books.Any())
			{
				pagination = new PaginationService<Book>(Books);
			}
		}


		#endregion

		#region Public Methods

		public async Task Delete(Book book)
		{

			if (await alertService.ShowConfirmation("Eliminar", $"Esta seguro de eliminar el libro {book.Title}?"))
			{
				try
				{
					await service.Delete(book.Id);
					Books.Remove(book);
					await alertService.ShowAlert("Eliminar Libro", $"Libro {book.Title} eliminado correctamente", "success");
				}
				catch (CustomHttpRequestException ex)
				{
					await alertService.ShowAlert("Eliminar Libro", $"error al eliminar el libro {book.Title}\n Error: {ex.Message}", "error");
				}
			}
		}
		public string TruncateText(string text, int wordCount)
		{
			if (string.IsNullOrEmpty(text))
				return string.Empty;

			var words = text.Split(' ').Take(wordCount).ToArray();
			return string.Join(" ", words) + (text.Split(' ').Length > wordCount ? "..." : "");
		}


		#endregion


		#region pagination methods

		private void GoToNextPage()
		{
			pagination?.NextPage();
		}

		private void GoToPreviousPage()
		{
			pagination?.PreviousPage();
		}

		#endregion



		private void FilterBooks()
		{
			var filteredBooks = Books;

			// Filtrar por texto de búsqueda si existe
			if (!string.IsNullOrWhiteSpace(SearchTerm))
			{
				filteredBooks = filteredBooks
					.Where(book => book.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)
								|| book.Author.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
					.ToList();
			}

			// Filtrar por estado si se ha seleccionado uno
			if (!string.IsNullOrWhiteSpace(SelectedStatus))
			{
				filteredBooks = filteredBooks
					.Where(book => book.Status == SelectedStatus)
					.ToList();
			}

			// Filtrar por stock si se ha seleccionado uno
			if (!string.IsNullOrWhiteSpace(SelectedInStock))
			{
				filteredBooks = filteredBooks
					.Where(book => book.InStock == bool.Parse(SelectedInStock))
					.ToList();
			}

			pagination.UpdateItems(filteredBooks);
		}

		private void OpenEditModal(Book book)
		{
			selectedBook = SelectBookEdit(book);
			showEditModal = true;
		}

		private void OpenDescriptionModal(Book book)
		{
			selectedBook = SelectBookEdit(book);
			showDescriptionModal = true;
		}

		private void OpenImageModal(Book book)
		{
			selectedBook = SelectBookEdit(book);
			showImageModal = true;
		}

		private async Task OpenSelectRandomBookModal()
		{
			UnReadBooks = (await service.GetAll()).Where(book => book.Status.Equals("no_leido")).ToList();
			showSelectRandomBookModal = true;
			StateHasChanged();
			await Task.Delay(1000); // Esperar para renderizar la ruleta
			string[] bookTitles = UnReadBooks.Select(b => b.Title).ToArray();
			await runtime.InvokeVoidAsync("initializeWheel", (object)bookTitles);
		}

		private Book SelectBookEdit(Book book)
		{
			return new Book
			{
				Id = book.Id,
				Title = book.Title,
				Author = book.Author,
				Price = book.Price,
				Status = book.Status,
				InStock = book.InStock,
				Description = book.Description,
				ImagePath = book.ImagePath,
				Raiting = book.Raiting,
				PersonalReview = book.PersonalReview
			};
		}

		private async void ChangeToRead(Book book)
		{
			book.Status = "leido";
			if (await alertService.ShowConfirmation("Cambio estado", $"Esta seguro de cambiar a estado leido el libro {book.Title}?"))
			{
				try
				{
					await service.Edit(book);
					await alertService.ShowAlert("Cambio Estado", $"Libro {book.Title} Estado leido", "success");
					navigation.NavigateTo($"/book-detail/{book.Id}");
				}
				catch (CustomHttpRequestException ex)
				{
					await alertService.ShowAlert("Editar Libro", $"error al editar el libro {book.Title}\n Error: {ex.Message}", "error");
				}

			}
		}



		private void CloseEditModal(bool isClosed)
		{
			showEditModal = false;
		}
		private void CloseImageModal(bool isClosed)
		{
			showImageModal = false;
		}
		private void CloseDescriptionModal(bool isClosed)
		{
			showDescriptionModal = false;
		}

		private void CloseSelectBook(bool isclosed)
		{
			showSelectRandomBookModal = false;
		}

		private async Task SaveBookChanges(Book updatedBook)
		{			
			var book = Books.FirstOrDefault(b => b.Id == updatedBook.Id);
			if (await alertService.ShowConfirmation("Editar", $"Esta seguro de editar el libro {book.Title}?"))
			{

				try
				{
					await service.Edit(updatedBook);
					await alertService.ShowAlert("Editar Libro", $"Libro {book.Title} editado correctamente", "success");
					if (book != null)
					{
						book.Title = updatedBook.Title;
						book.Author = updatedBook.Author;
						book.Price = updatedBook.Price;
						book.Status = updatedBook.Status;
						book.InStock = updatedBook.InStock;
						book.Description = updatedBook.Description;
						book.ImagePath = updatedBook.ImagePath;
						book.Raiting = updatedBook.Raiting;
						book.PersonalReview = book.PersonalReview;
					}
				}
				catch (CustomHttpRequestException ex)
				{
					await alertService.ShowAlert("Editar Libro", $"error al editar el libro {book.Title}\n Error: {ex.Message}", "error");
				}
			}


			showEditModal = false;
			showDescriptionModal = false;
		}

	}
}
