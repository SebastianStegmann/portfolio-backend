namespace WebServiceLayer.Models
{
    public class PersonListModel
    {
        public string? URL { get; set; }
        public required string Name { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
