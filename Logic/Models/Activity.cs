namespace Logic.Models
{
    public class Activity : Base
    {
        public string Name { get; set; }
        public virtual Project Project { get; set; }
    }
}
