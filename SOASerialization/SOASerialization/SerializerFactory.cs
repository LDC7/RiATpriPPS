using System;

namespace SOASerialization
{
    public static class SerializerFactory
    {
        public static ISerializer GetSerializer(string type)
        {
            switch (type)
            {
                case "Xml":
                    return new XmlSer();

                case "Json":
                    return new JsonSer();

                default:
                    throw new ArgumentException();
            }
        }
    }
}
