using System.ComponentModel.DataAnnotations;

namespace com.manager.front.model.book
{
	public class Book
	{

		public int Id { get; set; }

		[Required(ErrorMessage = "El título es obligatorio.")]
		[StringLength(100, ErrorMessage = "El título no puede superar los 100 caracteres.")]
		public string Title { get; set; }

		[StringLength(2000, ErrorMessage = "La descripción no puede superar los 2000 caracteres.")]
		public string Description { get; set; }


		[Required(ErrorMessage = "El autor es obligatorio.")]
		[StringLength(100, ErrorMessage = "El nombre del autor no puede superar los 100 caracteres.")]
		public string Author { get; set; }


		[Range(0.0, 10000000.0, ErrorMessage = "El precio debe ser inferior a 10'000.000")]
		public double Price { get; set; }

		[Required(ErrorMessage = "Debe seleccionar un estado.")]
		public string Status { get; set; }
		public bool InStock { get; set; }
		public string ImagePath { get; set; }

		[StringLength(2000, ErrorMessage = "La opinión personal no puede superar los 2000 caracteres.")]
		public string PersonalReview { get; set; }

		[Range(0, 10, ErrorMessage = "La nota debe ser entre 0  y 10")]
		public short Raiting { get; set; }
	}
}
