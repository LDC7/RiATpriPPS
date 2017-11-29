using System;
using System.IO;
using System.Net;

namespace SOAHttpClient
{
    //todo: выделите базвовый класс HttpClientBase в котором будет логика работы с хттп, здесь должны быть только функции Ping, GetInputData, WriteAnswer
    public class HttpClient
    {
        public bool Ping(string host, int port)
        {
            HttpStatusCode status;
            Request("GET", host, port, "Ping", out status);
            if (status == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

        public byte[] GetInputData(string host, int port)
        {
            HttpStatusCode status;
            return Request("GET", host, port, "GetInputData", out status);
        }

        public void WriteAnswer(string host, int port, byte[] outData)
        {
            HttpStatusCode status;
            Request("POST", host, port, "WriteAnswer", out status, outData);
        }

        private byte[] Request(string type, string host, int port, string method, out HttpStatusCode status, byte[] data = null)
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
