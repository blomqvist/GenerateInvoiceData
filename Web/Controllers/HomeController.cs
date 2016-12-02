using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Logic.Models;
using Web.Database;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(bool? previousMonth, string[] customers, string[] projects, string[] activities, string[] times)
        {
            if (projects.Length != times.Length)
            {
                return Content("Felaktig indata");
            }

            var dateRows = new List<DateRow>();
            using (var ctx = new Context())
            {
                for (var i = 0; i < projects.Length; ++i)
                {
                    if (customers[i] == null || projects[i] == null || activities[i] == null || times[i] == null)
                    {
                        continue;
                    }

                    var activity = ctx.Activities.FirstOrDefault(a => a.Name.Equals(activities[i], StringComparison.CurrentCultureIgnoreCase)) ?? new Activity() { Name = activities[i] };
                    var company = ctx.Companies.FirstOrDefault(c => c.Name.Equals(customers[i], StringComparison.CurrentCultureIgnoreCase)) ?? new Company() { Name = customers[i] };
                    var project = ctx.Projects.FirstOrDefault(p => p.Name.Equals(projects[i], StringComparison.CurrentCultureIgnoreCase)) ?? new Project() { Name = projects[i] };

                    if (activity.Id == 0)
                    {
                        ctx.Activities.Add(activity);
                    }

                    if (company.Id == 0)
                    {
                        ctx.Companies.Add(company);
                    }

                    if (project.Id == 0)
                    {
                        ctx.Projects.Add(project);
                    }

                    dateRows.Add(new DateRow()
                    {
                        Activity = activity,
                        Company = company,
                        Project = project,
                        NormalTime = times[i]
                    });

                    ctx.SaveChanges();
                }
            }

            var now = DateTime.Now;
            if (previousMonth.HasValue && previousMonth.Value)
            {
                now = new DateTime(now.Year, now.Month - 1, 1);
            }

            var di = DateInfo.Instance;
            await di.ReadDays(now.Year, now.Month);

            var currentDayOfMonth = new DateTime(now.Year, now.Month, 1);
            var daysInMonth = DateTime.DaysInMonth(now.Year, now.Month);
            var output = "";
            while (daysInMonth-- > 0)
            {
                if (!di.IsHoliday(currentDayOfMonth.Day))
                {
                    foreach (var row in dateRows)
                    {
                        row.Date = currentDayOfMonth;
                        output += $"{row}\n";
                    }
                }
                currentDayOfMonth = currentDayOfMonth.AddDays(1);
            }

            return Content(output);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
