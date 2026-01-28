namespace com.manager.front.service.factory.exception
{
	public class CustomHttpRequestException : Exception
	{
		public CustomHttpRequestException(string message, Exception innerException)
	   : base(message, innerException) { }
	}
}
