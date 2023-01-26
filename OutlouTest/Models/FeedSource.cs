using System.ComponentModel.DataAnnotations;

namespace OutlouTest.Models
{
    public class FeedSource
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 5)]
        public string Url { get; set; }
    }
}
