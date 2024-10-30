using Domain.Interfaces;

namespace Domain.Models
{
    public class VersionPath : IEntity
    {
        public int Id { get; set; }
        public string? ChangelogPath { get; set; }
        public string? ZipPath { get; set; }
        public int VersionInfoId { get; set; }
    }
}