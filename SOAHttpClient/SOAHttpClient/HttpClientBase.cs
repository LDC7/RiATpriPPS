using System;
using System.IO;
using System.Net;

namespace SOAHttpClient
{
    public abstract class HttpClientBase
    {
        protected byte[] Request(string type, string host, int port, string method, out HttpStatusCode status, byte[] data = null)
        {
            UriBuilder builder = new UriBuilder();
            builder.Host = host;
            builder.Port = port;
            builder.Path = method;

            var request = WebRequest.CreateHttp(builder.Uri);
            request.Timeout = 1000;
            request.Method = type;

            if (data != null)
            {
                using (var requestStream = request.GetRequestStream())
                {
                    requestStream.Write(data, 0, data.Length);
                }
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            status = response.StatusCode;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        responseStream.CopyTo(ms);
                        return ms.ToArray();
                    }
                }
            }

            return null;
        }
    }
}
