using Logic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Database;
using Web.JsonModels;

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

                    var company = ctx.Companies.FirstOrDefault(c => c.Name.Equals(customers[i], StringComparison.CurrentCultureIgnoreCase)) ?? new Company() { Name = customers[i] };
                    var project = ctx.Projects.FirstOrDefault(p => p.Name.Equals(projects[i], StringComparison.CurrentCultureIgnoreCase)) ?? new Project() { Name = projects[i] };
                    var activity = ctx.Activities.FirstOrDefault(a => a.Name.Equals(activities[i], StringComparison.CurrentCultureIgnoreCase)) ?? new Activity() { Name = activities[i] };

                    if (company.Id == 0)
                    {
                        ctx.Companies.Add(company);
                    }

                    if (activity.Id == 0)
                    {
                        ctx.Activities.Add(activity);
                    }

                    if (project.Id == 0)
                    {
                        ctx.Projects.Add(project);
                    }

                    activity.Project = project;
                    project.Company = company;

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

            var di = new DateInfo();
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

        [HttpGet, HttpPost]
        public async Task<JsonResult> AutoComplete(string customers = "", string projects = "", string activities = "")
        {
            customers = customers ?? string.Empty;
            projects = projects ?? string.Empty;
            activities = activities ?? string.Empty;

            var jsonResult = new Autocomplete();
            using (var ctx = new Context())
            {
                jsonResult.Customers = await ctx.Companies
                    .Where(x => x.Name.StartsWith(customers, StringComparison.CurrentCultureIgnoreCase))
                    .Select(x => x.Name)
                    .ToListAsync();

                jsonResult.Projects = await ctx.Projects
                    .Where(x => x.Name.StartsWith(projects, StringComparison.CurrentCultureIgnoreCase) 
                    && (!string.IsNullOrWhiteSpace(customers) ? x.Company.Name.StartsWith(customers, StringComparison.CurrentCultureIgnoreCase) : true))
                    .Select(x => x.Name)
                    .ToListAsync();

                jsonResult.Activities = await ctx.Activities
                    .Where(x => x.Name.StartsWith(activities, StringComparison.CurrentCultureIgnoreCase) 
                    && (!string.IsNullOrWhiteSpace(projects) ? x.Project.Name.StartsWith(projects, StringComparison.CurrentCultureIgnoreCase) : true))
                    .Select(x => x.Name)
                    .ToListAsync();
            }

            return new JsonResult(jsonResult);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
