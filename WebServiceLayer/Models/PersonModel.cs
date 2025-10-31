using WebServiceLayer.Models.DTO;

namespace WebServiceLayer.Models
{
    public class PersonModel
    {
        public string? URL { get; set; }
        public string Name { get; set; } = null!;
        public DateTime? Birthday { get; set; }
        public string? Location { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateTime? LastLogin { get; set; }
        public DateTime CreatedAt { get; set; }

        public string? SearchURL { get; set; }
        public string? BookmarkURL { get; set; }
        public string? IndividualRatingURL { get; set; }
    }
}
