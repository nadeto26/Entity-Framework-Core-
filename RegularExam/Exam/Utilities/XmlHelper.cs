using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Medicines.Utilities
{
    public static class XmlHelper
    {
        public static T Deserialize<T>(string inputXml, string rootName)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(T), xmlRoot);

            using StringReader reader = new StringReader(inputXml);
            T deserializedDtos =
                (T)xmlSerializer.Deserialize(reader);

            return deserializedDtos;
        }

        public static string Serialize<T>(T obj, string rootName)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRoot =
                new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(T), xmlRoot);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true
            };

            using (StringWriter writer = new StringWriter(sb))
            using (XmlWriter xmlWriter = XmlWriter.Create(writer, settings))
            {
                xmlSerializer.Serialize(xmlWriter, obj, namespaces);
            }

            return sb.ToString().TrimEnd();
        }
        //public static T DeserializeXML<T>(string inputXml, string rootName)
        //{
        //    XmlRootAttribute rootAttribute = new(rootName);

        //    XmlSerializer serializer = new XmlSerializer(typeof(T), rootAttribute);

        //    using StringReader stringReader = new(inputXml);

        //    return (T)serializer.Deserialize(stringReader)!;
        //}

        //public static T DeserializeXML<T>(string inputXml)
        //{
        //    XmlSerializer serializer = new XmlSerializer(typeof(T));

        //    using StringReader stringReader = new(inputXml);

        //    return (T)serializer.Deserialize(stringReader)!;
        //}

        //public static string SerializeXML<T>(this T obj, string rootName)
        //{
        //    StringBuilder sb = new();

        //    XmlRootAttribute xmlRoot =
        //        new(rootName);

        //    XmlSerializer serializer =
        //        new(obj.GetType(), xmlRoot);

        //    XmlSerializerNamespaces serializerNamespaces =
        //        new();

        //    serializerNamespaces
        //        .Add(string.Empty, string.Empty);

        //    using StringWriter writer =
        //        new(sb);

        //    serializer
        //        .Serialize(writer, obj, serializerNamespaces);

        //    return sb
        //        .ToString()
        //        .TrimEnd();
        //}

        //public static string SerializeXML<T>(this T[] obj, string rootName)
        //{
        //    StringBuilder sb = new();

        //    XmlRootAttribute xmlRoot =
        //        new(rootName);

        //    XmlSerializer serializer =
        //        new(obj.GetType(), xmlRoot);

        //    XmlSerializerNamespaces serializerNamespaces =
        //        new();

        //    serializerNamespaces
        //        .Add(string.Empty, string.Empty);

        //    using StringWriter writer =
        //        new(sb);

        //    serializer
        //        .Serialize(writer, obj, serializerNamespaces);

        //    return sb
        //        .ToString()
        //        .TrimEnd();
        //}

        //public static string SerializeToXml<T>(this T obj, string rootName)
        //{
        //    var xmlSerializer = new XmlSerializer(typeof(T), new XmlRootAttribute(rootName));

        //    var namespaces = new XmlSerializerNamespaces();
        //    namespaces.Add(string.Empty, string.Empty);

        //    string result = null;

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        xmlSerializer.Serialize(ms, obj, namespaces);

        //        result = Encoding.UTF8.GetString(ms.ToArray());
        //    }

        //    return result;
        //}

        //public static T DeserializeFromXml<T>(this string xmlString, string rootName)
        //{
        //    var xmlSerializer = new XmlSerializer(typeof(T), new XmlRootAttribute(rootName));

        //    T result = default(T);

        //    using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(xmlString)))
        //    {
        //        result = (T)xmlSerializer.Deserialize(ms);
        //    }

        //    return result;
        //}
    }
}
