using System.Xml.Serialization;

namespace Infrastructure.Extensions
{
    public static class XmlExtensions
    {
        public static string ToXml<T>(this T obj)
        {
            var serializer = new XmlSerializer(typeof(T));

            using var stream = new StringWriter();

            serializer.Serialize(stream, obj);

            return stream.ToString();
        }
    }
}
