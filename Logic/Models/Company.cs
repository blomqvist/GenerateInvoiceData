using System.Collections.Generic;

namespace Logic.Models
{
    public class Company : Base
    {
        public string Name { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
    }
}
