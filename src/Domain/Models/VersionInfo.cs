using Domain.Interfaces;
using System.Xml.Serialization;

namespace Domain.Models
{
    [XmlRoot("item")]
    public class VersionInfo : IEntity
    {
        [XmlElement("id")]
        public int Id { get; set; }
        [XmlElement("version")]
        public required string Version { get; set; }
        [XmlElement("releaseDate")]
        public DateTime ReleaseDate { get; set; }
        [XmlElement("mandatory")]
        public bool IsMandatory { get; set; }
        [XmlElement("available")]
        public bool IsAvailable { get; set; }
        [XmlElement("changelog")]
        public string? ChangelogUrl { get; set; }
        [XmlElement("url")]
        public string? ReleaseUrl { get; set; }
        [XmlElement("applicationId")]
        public int ApplicationId { get; set; }
    }
}