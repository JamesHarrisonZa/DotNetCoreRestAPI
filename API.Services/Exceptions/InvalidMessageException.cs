using System;

namespace API.Services.Exceptions
{
	public class InvalidMessageException : Exception
	{
		public InvalidMessageException(string message)
			: base(message)
		{
		}
	}
}
