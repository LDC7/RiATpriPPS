using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Reflection;
using SOASerialization;

namespace Listener
{
    public class HttpListenerImpl : HttpListenerBase
    {
        private Queue answersQueue;
        ConcurrentDictionary<string, MethodInfo> methods;

        public HttpListenerImpl(string host, string port) : base(host, port)
        {
            answersQueue = new Queue();
            methods = new ConcurrentDictionary<string, MethodInfo>(from m in typeof(HttpListenerImpl).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                                                                   select new KeyValuePair<string, MethodInfo>(m.Name, m));
        }

        protected override void ProcessQuery(string methodName, HttpListener httpListener)
        {
            MethodInfo method;

            if (methods.TryGetValue(methodName, out method))
            {
                method.Invoke(this, new object[0]);
            }
            else
            {
                WriteToResponse(HttpStatusCode.BadRequest, $"Unknown method {methodName}");
            }
        }

        private void PostInputData()
        {
            Input input = GetFromRequestBody<Input>();

            answersQueue.Enqueue(InputToOutputFunc(input));

            WriteToResponse(HttpStatusCode.OK, string.Empty);
        }

        private void GetAnswer()
        {
            if (answersQueue.Count > 0)
            {
                Output output = (Output)answersQueue.Dequeue();
                WriteToResponse(HttpStatusCode.OK, output);
            }
            else
            {
                WriteToResponse(HttpStatusCode.NotAcceptable, "Answers collection is empty");
            }
        }

        private static Output InputToOutputFunc(Input data)
        {
            var outData = new Output();
            List<decimal> list = new List<decimal>();

            foreach (var d in data.Sums)
            {
                list.Add(d);
                outData.SumResult += d;
            }
            outData.SumResult *= data.K;

            if (data.Muls.Length > 0)
                outData.MulResult = 1;
            foreach (var d in data.Muls)
            {
                list.Add(d);
                outData.MulResult *= d;
            }

            list.Sort();
            outData.SortedInputs = list.ToArray();

            return outData;
        }
    }
}