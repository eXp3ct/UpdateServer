namespace Domain.Dtos
{
    public class VersionInfoDto
    {
        public required string Version { get; set; }
        public bool IsMandatory { get; set; }
        public bool IsAvailable { get; set; }
    }
}
