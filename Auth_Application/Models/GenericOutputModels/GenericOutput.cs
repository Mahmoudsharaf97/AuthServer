namespace Auth_Application.Models
{
	public class GenericOutput<TResult>
	{
		public GenericOutput()
		{
			ErrorDetails = new();
		}
		public TResult Result { get; set; }
		public ErrorDetails ErrorDetails { get; set; }

	}
}
