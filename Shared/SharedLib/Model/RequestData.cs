using System.ComponentModel.DataAnnotations;

namespace SubredditStats.Shared.Model
{
    public class RequestData
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int Count { get; set; }
    }
}
