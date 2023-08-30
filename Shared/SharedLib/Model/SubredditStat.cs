using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubredditStats.Shared.Model
{
    public class SubredditStat
    {
        public string Value { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public SubredditStat(string value, string name, string description)
        {
            Value = value;
            Name = name;
            Description = description;
        }
    }
}
