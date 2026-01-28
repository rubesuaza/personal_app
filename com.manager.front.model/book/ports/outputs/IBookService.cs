namespace com.manager.front.model.book.ports.outputs
{
	public interface IBookService
	{
		Task<Book> GetById(int id);
		Task<Book> Create(Book book);
		Task<List<Book>> GetAll();
		Task Delete(int id);
		Task<Book> Edit(Book book);


	}
}
