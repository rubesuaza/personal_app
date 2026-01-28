using com.manager.front.helpers;
using com.manager.front.model.book.ports.outputs;
using com.manager.front.service.bookService;
using com.manager.front.service.bookService.repository;
using com.manager.front.service.factory;
using com.manager.front.service.file;
using Microsoft.Extensions.Options;

namespace com.manager.front.Configurations
{
	public class DependencyInjectionConf
	{
		public static void DependencyInjectionConfServices(IServiceCollection services)
		{

			services.AddScoped<IBookRepository>(provider =>
			{
				var options = provider.GetRequiredService<IOptions<ApiUrlOption>>().Value;
				return new BookRepository(provider.GetRequiredService<HttpClient>(), options.Books);
			});

			services.AddScoped<IBookService, BookService>();

			services.AddScoped<SweetAlertService>();

			services.AddScoped<IBookFileService, PdfBookService>();

		}

	}
}
