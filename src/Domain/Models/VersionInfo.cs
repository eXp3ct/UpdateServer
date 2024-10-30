using Domain.Interfaces;

namespace Domain.Models
{
    public class VersionInfo : IEntity
    {
        public int Id { get; set; }
        public required string Version { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool IsMandatory { get; set; }
        public bool IsAvailable { get; set; }
        public string? ChangelogUrl { get; set; }
        public string? ReleaseUrl { get; set; }
        public int ApplicationId { get; set; }
    }
}