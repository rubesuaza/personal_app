using com.manager.front.model.book;
using com.manager.front.model.book.ports.outputs;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.Rendering;


namespace com.manager.front.service.file
{
	public class PdfBookService : IBookFileService
	{

		public byte[] GenerateBookDetailPdf(Book book, string imagePath)
		{
			try
			{

				// Crear documento
				Document document = new Document();
				DefineStyles(document); // Definir estilos

				Section section = document.AddSection();
				section.PageSetup.TopMargin = "2cm";
				section.PageSetup.BottomMargin = "2cm";
				section.PageSetup.LeftMargin = "2cm";
				section.PageSetup.RightMargin = "2cm";

				// Agregar imagen de portada si existe
				if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
				{
					var image = section.AddImage(imagePath);
					image.Width = "6cm";
					image.LockAspectRatio = true;
					image.Left = ShapePosition.Center;
					image.Top = ShapePosition.Top;
					section.AddParagraph("\n"); // Espacio después de la imagen
				}

				// Título y Autor
				section.AddParagraph($"{book.Title}", "Title");
				section.AddParagraph($"Autor: {book.Author}", "Subtitle");

				// Detalles
				section.AddParagraph("\nCalificación: " + book.Raiting + " / 10", "NormalBold");
				section.AddParagraph("\nDescripción: \n", "NormalBold");
				section.AddParagraph(book.Description, "Normal");
				section.AddParagraph("\nOpinión personal:\n", "NormalBold");
				section.AddParagraph(book.PersonalReview ?? "", "Normal");
				section.AddParagraph("\nCosto: " + book.Price.ToString("C"), "NormalBold");

				// Renderizar el PDF
				PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(true);
				pdfRenderer.Document = document;
				pdfRenderer.RenderDocument();

				using (MemoryStream stream = new MemoryStream())
				{
					pdfRenderer.PdfDocument.Save(stream, false);
					return stream.ToArray();
				}

			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				throw;
			}
		}
		// Definir estilos
		private void DefineStyles(Document document)
		{
			Style style = document.Styles["Normal"];
			style.Font.Name = "Arial";
			style.Font.Size = 11;

			Style titleStyle = document.Styles.AddStyle("Title", "Normal");
			titleStyle.Font.Size = 18;
			titleStyle.Font.Bold = true;
			titleStyle.ParagraphFormat.Alignment = ParagraphAlignment.Center;
			titleStyle.ParagraphFormat.SpaceAfter = "0.5cm";

			Style subtitleStyle = document.Styles.AddStyle("Subtitle", "Normal");
			subtitleStyle.Font.Size = 14;
			subtitleStyle.Font.Italic = true;
			subtitleStyle.ParagraphFormat.Alignment = ParagraphAlignment.Center;
			subtitleStyle.ParagraphFormat.SpaceAfter = "0.5cm";

			Style normalBold = document.Styles.AddStyle("NormalBold", "Normal");
			normalBold.Font.Bold = true;
		}

	}
}
