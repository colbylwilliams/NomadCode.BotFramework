using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Rest;

namespace NomadCode.BotFramework
{
    public class DirectLineClientCredentials : ServiceClientCredentials
    {
        public string Authorization { get; private set; }

        public string Endpoint { get; private set; }

        public string Secret { get; private set; }

        public DirectLineClientCredentials (string secret = null, string endpoint = null)
        {
            Secret = secret;
            Authorization = secret;
            Endpoint = endpoint ?? @"https://directline.botframework.com/";
        }

        public override Task ProcessHttpRequestAsync (HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue ("Bearer", Authorization);

            return base.ProcessHttpRequestAsync (request, cancellationToken);
        }
    }
}
