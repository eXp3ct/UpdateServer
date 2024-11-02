namespace Domain.Dtos
{
    public class VersionInfoDto
    {
        public int ApplicationId { get; set; }
        public string Version { get; set; }
        public bool IsMandatory { get; set; }
        public bool IsAvailable { get; set; }
    }
}