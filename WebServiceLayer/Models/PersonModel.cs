using WebServiceLayer.Models.DTO;

namespace WebServiceLayer.Models
{
    public class PersonModel
    {
        public string? URL { get; set; }
        public required string Name { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Location { get; set; }
        public required string Email { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime CreatedAt { get; set; }

        public string? SearchURL { get; set; }
        public string? BookmarkURL { get; set; }
        public string? IndividualRatingURL { get; set; }
    }
}
