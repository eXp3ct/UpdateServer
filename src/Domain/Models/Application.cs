using Domain.Interfaces;

namespace Domain.Models
{
    public class Application : IEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTime DateOfCreation { get; set; }
        public DateTime? DateModified { get; set; }
    }
}