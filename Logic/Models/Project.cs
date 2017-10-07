using System.Collections.Generic;

namespace Logic.Models
{
    public class Project : Base
    {
        public string Name { get; set; }

        public virtual ICollection<Activity> Activities { get; set; }
        public virtual Company Company { get; set; }
    }
}
