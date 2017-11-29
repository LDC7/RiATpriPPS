using System;
using SOASerialization;
using System.Collections.Generic;
using System.Text;

namespace SOAHttpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Input inData;
            Output outData;
            HttpClientImpl http = new HttpClientImpl();
            ISerializer serializer = SerializerFactory.GetSerializer("Json");
            string host = "127.0.0.1";
            int port;

            if (int.TryParse(Console.ReadLine(), out port))
            {
                if (http.Ping(host, port))
                {
                    inData = serializer.Deserialize<Input>(Encoding.UTF8.GetString(http.GetInputData(host, port)));
                    outData = InputToOutputFunc(inData);
                    http.WriteAnswer(host, port, Encoding.UTF8.GetBytes(serializer.Serialize(outData)));
                }
            }
        }

        public static Output InputToOutputFunc(Input data)
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
