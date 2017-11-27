using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using Serializer;

namespace Listener
{
    public abstract class HttpListenerBase : IHttpListener
    {
        private HttpListenerContext context = null;
        private ISerializer serializer;
        private HttpListener httpListener;
        private string host;
        private string port;

        public HttpListenerBase(string host, string port)
        {
            serializer = new Serializer.Serializer();
            this.host = host;
            this.port = port;
        }

        public void Start()
        {
            httpListener = new HttpListener();
            httpListener.Prefixes.Add($"http://{host}:{port}/");

            httpListener.Start();

            while (httpListener.IsListening)
            {
                context = httpListener.GetContext();
                Console.WriteLine(context.Request.Url);
                var uri = context.Request.RawUrl;
                var methodName = GetMethodName(uri);

                ProcessQuery(methodName, httpListener);
            }
        }

        protected void Ping()
        {
            WriteToResponse(HttpStatusCode.OK, string.Empty);
        }

        protected void Stop()
        {
            if(httpListener != null && httpListener.IsListening)
            {
                WriteToResponse(HttpStatusCode.OK, string.Empty);
                httpListener.Stop();
            }
        }

        protected abstract void ProcessQuery(string methodName, HttpListener httpListener);

        protected NameValueCollection GetParameters()
        {
            return context.Request.QueryString;
        }

        protected T GetFromRequestBody<T>()
        {
            using (var sr = new StreamReader(context.Request.InputStream))
            {
                var bodyStr = sr.ReadToEnd();
                return serializer.Deserialize<T>(Encoding.UTF8.GetBytes(bodyStr));
            }
        }

        protected void WriteToResponse(HttpStatusCode httpStatusCode, string response)
        {
            context.Response.StatusCode = (int)httpStatusCode;
            using (var sw = new StreamWriter(context.Response.OutputStream))
            {
                sw.WriteLine(response);
            }
        }

        protected void WriteToResponse<T>(HttpStatusCode httpStatusCode, T response)
        {
            WriteToResponse(httpStatusCode, Encoding.UTF8.GetString(serializer.Serialize(response)));
        }

        protected static string GetMethodName(string uri)
        {
            var result = uri.Substring(1, uri.Length - 1);
            var questionCharIndex = result.IndexOf("?", 0, StringComparison.Ordinal);
            if (questionCharIndex == -1)
            {
                return result;
            }

            return result.Substring(0, questionCharIndex);
        }
    }
}