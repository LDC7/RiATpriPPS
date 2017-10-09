using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace SOASerialization
{
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
                    MemoryStream stream = new MemoryStream();
                    Xml.Serialize(stream, outData);

                    stream.Position = 0;
                    string outString = (new StreamReader(stream)).ReadToEnd();
                    outString = outString.Substring(outString.IndexOf('<', 1));
                    outString = outString.Remove(
                        outString.IndexOf(' ')
                        ,outString.IndexOf('>', outString.IndexOf(' '))  - outString.IndexOf(' '));
                    outString = outString.Replace(" ", string.Empty);
                    outString = outString.Replace(Environment.NewLine, string.Empty);

                    //Console.WriteLine(XMLSTR);
                    Console.Write(outString);
                    break;

                case JSONSTR:
                    var sb = new StringBuilder();
                    string line;
                    while ((line = Console.ReadLine()) != null)
                    {
                        sb.Append(line);
                    }

                    inData = JsonConvert.DeserializeObject<Input>(sb.ToString());
                    outData = InputToOutputFunc(inData);
                    var outStr = JsonConvert.SerializeObject(outData);

                    //Console.WriteLine(JSONSTR);
                    Console.Write(outStr);
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

