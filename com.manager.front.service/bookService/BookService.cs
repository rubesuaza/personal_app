using com.manager.front.model.book;
using com.manager.front.model.book.ports.outputs;
using com.manager.front.service.bookService.repository;
using com.manager.front.service.factory.exception;

namespace com.manager.front.service.bookService
{
	public class BookService : IBookService
	{
		private readonly IBookRepository repository;

		public BookService(IBookRepository repository)
		{
			this.repository = repository;
		}

		public async Task<Book> Create(Book book)
		{
			var response = await repository.AddAsync(book, "create");
			if ((response.Status.Equals("success")))
			{
				return response.Data;
			}
			throw new CustomHttpRequestException(response?.Message, null);
		}

		public async Task Delete(int id)
		{
			await repository.DeleteAsync(id, "delete");
		}

		public async Task<Book> Edit(Book book)
		{
			var response = await repository.UpdateAsync(book, "edit");
			if ((response.Status.Equals("success")))
			{
				return response.Data;
			}
			throw new CustomHttpRequestException(response?.Message, null);
		}

		public async Task<List<Book>> GetAll()
		{
			var response = await repository.GetAllAsync("getAll");
			return (List<Book>)response.Data;
		}

		public async Task<Book> GetById(int id)
		{
			var response = await repository.GetByIdAsync($"get?id={id}");
			return (Book)response.Data;
		}
	}
}
