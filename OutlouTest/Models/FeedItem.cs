using System.ComponentModel.DataAnnotations;

namespace OutlouTest.Models
{
    public class FeedItem
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 5)]
        public string Link { get; set; }

        public string Summary { get; set; }

        public DateTime PublishDate { get; set; }

        public bool IsRead { get; set; } = false;
    }
}
