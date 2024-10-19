using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Infrastructure.Extensions
{
    public static class XmlExtensions
    {
        public static string SerializeToXml<T>(T obj)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));

            var settings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                Indent = true,
                OmitXmlDeclaration = false // Оставляем декларацию XML
            };

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty); // Убираем лишние пространства имен

            using var stringWriter = new StringWriter();
            using var xmlWriter = XmlWriter.Create(stringWriter, settings);

            xmlSerializer.Serialize(xmlWriter, obj, namespaces);
            return stringWriter.ToString();
        }
    }
}
