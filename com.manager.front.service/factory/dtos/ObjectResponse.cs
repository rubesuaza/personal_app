namespace com.manager.front.service.factory.dtos
{
	public class ObjectResponse<T>
	{
		public string Status { get; set; } // success, error
		public T Data { get; set; }        // Datos de respuesta
		public string Message { get; set; }
	}
}
