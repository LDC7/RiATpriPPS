using System.Net;

namespace SOAHttpClient
{
    public class HttpClientImpl : HttpClientBase
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
    }
}
