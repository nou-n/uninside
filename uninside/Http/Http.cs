using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace uninside.Http
{
    internal static class Http
    {
        static private HttpClient httpClient;

        static Http()
        {
            httpClient = new HttpClient();
        }

        public static string UrlEncode(Dictionary<string, string> parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in parameters)
            {
                if (sb.Length > 0) sb.Append("&");
                sb.Append(WebUtility.UrlEncode(kvp.Key));
                sb.Append("=");
                sb.Append(WebUtility.UrlEncode(kvp.Value));
            }
            return sb.ToString();
        }

        async public static Task<HttpResponse> GetAsync(string url, Dictionary<string, string> headers = null)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, new Uri(url));

            if (headers != null) foreach (KeyValuePair<string, string> header in headers) request.Headers.TryAddWithoutValidation(header.Key, header.Value);

            return new HttpResponse(await httpClient.SendAsync(request));
        }

        async public static Task<HttpResponse> PostAsync(string url, object payload = null, Dictionary<string, string> headers = null)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri(url));

            if (payload != null)
            {
                if (payload is string stringValue) request.Content = new StringContent(stringValue);
                else if (payload is MultipartFormDataContent multipartContent) request.Content = multipartContent;
                else if (payload is byte[] byteArray) request.Content = new ByteArrayContent(byteArray);
                else throw new ArgumentException("지원되지 않는 타입입니다. payload는 string, MultipartFormDataContent 또는 byte[] 타입이어야 합니다.");
            }
            else
            {
                request.Content = new StringContent(string.Empty);
            }

            if (headers != null)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    if (header.Key.Equals("content-type", StringComparison.OrdinalIgnoreCase) && request.Content != null)
                    {
                        request.Content.Headers.ContentType = new MediaTypeHeaderValue(header.Value);
                        continue;
                    }
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            return new HttpResponse(await httpClient.SendAsync(request));
        }
    }

    internal class HttpResponse
    {
        public HttpResponseMessage Message { get; }
        public Dictionary<string, string[]> Headers { get; private set; }
        public int StatusCode { get; private set; }

        public HttpResponse(HttpResponseMessage response)
        {
            this.Message = response;

            Headers = new Dictionary<string, string[]>();
            foreach (KeyValuePair<string, IEnumerable<string>> header in response.Headers)
            {
                Headers[header.Key] = header.Value.ToArray();
            }

            StatusCode = (int)response.StatusCode;
        }
    }
}
