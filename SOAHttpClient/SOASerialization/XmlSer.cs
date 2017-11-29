using System;
using System.IO;
using System.Xml.Serialization;

namespace SOASerialization
{
    public class XmlSer : ISerializer
    {
        public T Deserialize<T>(string str)
        {
            T obj;
            var Serializer = new XmlSerializer(typeof(T));
            obj = (T)Serializer.Deserialize(new StringReader(str));

            return obj;
        }

        public string Serialize<T>(T obj)
        {
            var Serializer = new XmlSerializer(typeof(Output));
            MemoryStream stream = new MemoryStream();
            Serializer.Serialize(stream, obj);

            stream.Position = 0;
            string str = (new StreamReader(stream)).ReadToEnd();

            str = str.Substring(str.IndexOf('<', 1));
            str = str.Remove(
                str.IndexOf(' ')
                , str.IndexOf('>', str.IndexOf(' ')) - str.IndexOf(' '));
            str = str.Replace(" ", string.Empty);
            str = str.Replace(Environment.NewLine, string.Empty);

            return str;
        }
    }
}
