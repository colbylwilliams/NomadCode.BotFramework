using System.Net.Http;

namespace NomadCode.BotFramework
{
	public partial class DirectLineClient
	{
		public DirectLineClient (string secretOrToken = null, params DelegatingHandler [] handlers) : this (handlers)
		{
			Credentials = new DirectLineClientCredentials (secretOrToken);
		}
	}
}
