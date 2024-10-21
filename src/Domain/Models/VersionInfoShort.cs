namespace Domain.Models
{
    public class VersionInfoShort
    {
        public Guid Id { get; set; }
        public DateTime ReleaseDate { get; set; }
        public required string Version { get; set; }
        public bool IsMandatory { get; set; }
        public bool IsActive { get; set; }
    }
}
