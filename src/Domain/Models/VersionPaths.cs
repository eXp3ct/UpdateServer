namespace Domain.Models
{
    public class VersionPaths
    {
        public Guid VersionInfoId { get; set; }
        public string ChangelogPath { get; set; }
        public string ZipPath { get; set; }

        public VersionInfo VersionInfo { get; set; } = null!;
    }
}
