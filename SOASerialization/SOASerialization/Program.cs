using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;


namespace SOASerialization
{
    [Serializable]
    [DataContract]
    public class Input
    {
        [DataMember]
        public int K { get; set; }

        [DataMember]
        public decimal[] Sums { get; set; }

        [DataMember]
        public int[] Muls { get; set; }
    }

    [Serializable]
    [DataContract]
    public class Output
    {
        [DataMember]
        public decimal SumResult { get; set; }

        [DataMember]
        public int MulResult { get; set; }

        [DataMember]
        public decimal[] SortedInputs { get; set; }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            const string XMLSTR = "Xml";
            const string JSONSTR = "Json";

            Input inData;
            Output outData;
            string type = Console.ReadLine();

            switch (type)
            {
                case XMLSTR:
                    XmlSerializer Xml = new XmlSerializer(typeof(Input));
                    inData = (Input)Xml.Deserialize(Console.In);
                    outData = InputToOutputFunc(inData);
                    Xml = new XmlSerializer(typeof(Output));
                    Console.WriteLine(XMLSTR);
                    Xml.Serialize(Console.Out, outData);
                    break;

                case JSONSTR:
                    DataContractJsonSerializer Json = new DataContractJsonSerializer(typeof(Input));
                    //inData = (Input)Json.ReadObject(Console.OpenStandardInput());

                    var sb = new StringBuilder();
                    string line;
                    while ((line = Console.ReadLine()) != null)
                    {
                        sb.Append(line);
                    }

                    using (Stream s = new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString())))
                    {
                        inData = (Input)Json.ReadObject(s);
                    }

                    outData = InputToOutputFunc(inData);
                    Json = new DataContractJsonSerializer(typeof(Output));
                    Console.WriteLine(JSONSTR);
                    Json.WriteObject(Console.OpenStandardOutput(), outData);
                    break;
            }

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

