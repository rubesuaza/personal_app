namespace com.manager.front.model.book.ports.outputs
{
	public interface IBookFileService
	{
		byte[] GenerateBookDetailPdf(Book book, string imagePath);
	}
}
