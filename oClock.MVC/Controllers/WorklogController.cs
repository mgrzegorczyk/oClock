using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using oClock.MVC.Infrastructure;

namespace oClock.MVC.Controllers;

public class WorklogController : Controller
{
    private readonly JiraDbContext _context;
    private readonly List<QualificationGroup> _qualificationGroups;

    public WorklogController(JiraDbContext context, IConfiguration configuration)
    {
        _context = context;
        var json = System.IO.File.ReadAllText("qualificationGroups.json");
        _qualificationGroups = JsonConvert.DeserializeObject<List<QualificationGroup>>(json);
    }

    public IActionResult Index(DateTime? startDate, DateTime? endDate, string author)
    {
        if (!startDate.HasValue)
        {
            startDate = new DateTime(DateTime.Now.Year, 1, 1); // Początek roku
        }
        if (!endDate.HasValue)
        {
            endDate = DateTime.Now; // Dziś
        }

        var worklogs = _context.Worklogs
            .Where(w => w.WorklogDate >= startDate && w.WorklogDate <= endDate)
            .ToList();

        var authors = worklogs.Select(w => w.Author).Distinct().ToList();

        var teamData = worklogs.GroupBy(w => w.Qualification)
            .Select(g => new 
            {
                Qualification = g.Key,
                TotalHours = g.Sum(w => w.TimeSpent),
                Percentage = g.Sum(w => w.TimeSpent) / worklogs.Sum(w => w.TimeSpent) * 100
            }).ToList();

        List<dynamic> selectedAuthorData = new List<dynamic>();
        if (!string.IsNullOrEmpty(author))
        {
            selectedAuthorData = worklogs
                .Where(w => w.Author == author)
                .GroupBy(w => w.Qualification)
                .Select(g => new 
                {
                    Qualification = g.Key,
                    TotalHours = g.Sum(w => w.TimeSpent),
                    Percentage = g.Sum(w => w.TimeSpent) / worklogs.Where(w => w.Author == author).Sum(w => w.TimeSpent) * 100
                }).ToList<dynamic>();
        }

        ViewBag.StartDate = startDate.Value.ToString("yyyy-MM-dd");
        ViewBag.EndDate = endDate.Value.ToString("yyyy-MM-dd");
        ViewBag.TeamData = teamData;
        ViewBag.IndividualData = selectedAuthorData;
        ViewBag.Authors = authors;
        ViewBag.SelectedAuthor = author;
        ViewBag.QualificationGroups = _qualificationGroups;

        return View();
    }
}