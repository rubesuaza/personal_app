using Microsoft.AspNetCore.Components.Forms;

namespace com.manager.front.helpers
{
	public static class FileUploadHelper
	{
		private static readonly string[] AllowedFormats = { ".png", ".jpg", ".jpeg" };
		private const long MaxFileSize = 5 * 1024 * 1024; // 5MB

		public static async Task<(bool IsValid, string Message, string ImagePath)> ValidUploadImageAsync(InputFileChangeEventArgs e, IWebHostEnvironment env)
		{
			var imageFile = e.File;

			if (imageFile == null)
				return (false, "Debe seleccionar un archivo.", null);

			// Validar tamaño
			if (imageFile.Size > MaxFileSize)
				return (false, "El tamaño de la imagen no puede superar los 5MB.", null);

			// Validar formato
			var fileExtension = Path.GetExtension(imageFile.Name).ToLower();
			if (!AllowedFormats.Contains(fileExtension))
				return (false, "Formato no permitido. Solo PNG, JPG o JPEG.", null);

			// Generar nombre único y ruta de almacenamiento
			var fileName = $"{Guid.NewGuid()}{fileExtension}";
			var filePath = Path.Combine(env.WebRootPath, "images", fileName);


			// Guardar la imagen
			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				await imageFile.OpenReadStream().CopyToAsync(fileStream);
			}

			// Retornar la ruta relativa de la imagen
			string imagePath = $"images/{fileName}";
			return await Task.Run(() => (true, "Imagen subida correctamente.", imagePath));
		}

		public static void DiscardImage(String imagePath, IWebHostEnvironment env)
		{
			var filePath = Path.Combine(env.WebRootPath, imagePath);
			if (File.Exists(filePath))
			{
				File.Delete(filePath);
			}
		}


	}
}
