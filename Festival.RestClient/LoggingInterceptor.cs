using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Festival.RestClient
{
    /// <summary>
    /// Custom interceptor for HTTP requests and responses.
    /// Fulfills the requirement to log all performed HTTP steps.
    /// </summary>
    public class LoggingInterceptor : DelegatingHandler
    {
        public LoggingInterceptor(HttpMessageHandler innerHandler) : base(innerHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Console.WriteLine("\n=======================================================");
            Console.WriteLine($"[INTERCEPTOR - REQUEST OUT] {request.Method} {request.RequestUri}");
            
            if (request.Content != null)
            {
                string requestBody = await request.Content.ReadAsStringAsync(cancellationToken);
                Console.WriteLine($"[PAYLOAD]: {requestBody}");
            }

            // Proceed with the actual network transmission
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            Console.WriteLine($"\n[INTERCEPTOR - RESPONSE IN] {(int)response.StatusCode} {response.ReasonPhrase}");
            
            if (response.Content != null)
            {
                string responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
                Console.WriteLine($"[BODY]: {responseBody}");
            }
            Console.WriteLine("=======================================================\n");

            return response;
        }
    }
}