namespace Auth_Application.Models
{
	public class GenericOutput<TResult>
	{
		public GenericOutput()
		{
			errorDetails = new();
		}
		public TResult result { get; set; }
		public ErrorDetails errorDetails { get; set; }

	}
}
