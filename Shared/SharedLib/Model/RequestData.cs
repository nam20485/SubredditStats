using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubredditStats.Shared.Model
{
    public class RequestData
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int Count { get; set; }
    }
}
