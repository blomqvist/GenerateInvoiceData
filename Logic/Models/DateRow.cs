//using Common;
using System;

namespace Logic.Models
{
    public class DateRow
    {
        public DateTime Date { get; set; }

        public Company Company { get; set; }

        public Project Project { get; set; }

        public Activity Activity { get; set; }

        public string NormalTime { get; set; } = "0";

        public double Overtimex15 { get; set; } = 0;

        public double Overtimex2 { get; set; } = 0;

        public override string ToString()
        {
            return /*Date.ToShortDateString() +*/ $"\t{Company.Name}\t{Project.Name}\t{Activity.Name}\t\t\t\t{NormalTime}\t{Overtimex15}\t{Overtimex2}";
        }
    }
}
