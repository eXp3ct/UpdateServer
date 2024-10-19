using System.Xml.Serialization;

namespace Domain.Models
{
    [XmlRoot("item")]
    public class VersionInfo
    {
        [XmlElement("id")]
        public Guid Id { get; set; }

        [XmlElement("applicationName")]
        public required string ApplicationName { get; set; }

        [XmlElement("version")]
        public required string Version { get; set; }

        [XmlElement("releaseDate")]
        public DateTime ReleaseDate { get; set; }

        [XmlElement("changelog")]
        public required string ChangelogFileUrl { get; set; }

        [XmlElement("url")]
        public required string ZipUrl { get; set; }

        [XmlElement("mandatory")]
        public bool IsMandatory { get; set; }

        [XmlElement("isActive")]
        public bool IsActive { get; set; }
    }
}
