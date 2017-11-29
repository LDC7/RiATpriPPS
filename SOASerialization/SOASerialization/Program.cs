using System;
using System.Collections.Generic;
using System.Text;

namespace SOASerialization
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Input inData;
            Output outData;
            var type = Console.ReadLine();
            ISerializer Ser = SerializerFactory.GetSerializer(type);

            var sb = new StringBuilder();
            string line;
            while ((line = Console.ReadLine()) != null)
            {
                sb.Append(line);
            }

            inData = Ser.Deserialize<Input>(sb.ToString());

            outData = InputToOutputFunc(inData);

            string outStr = Ser.Serialize<Output>(outData);

            Console.Write(outStr);
            Console.ReadKey();
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

