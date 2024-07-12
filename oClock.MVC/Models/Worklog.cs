using System.ComponentModel.DataAnnotations;
using oClock.MVC.Enums;

namespace oClock.MVC.Models;

public class Worklog
{
    [Key]
    public int Ref { get; set; }
    public string Author { get; set; }
    public string Project { get; set; }
    public string Issue { get; set; }
    public string IssueSummary { get; set; }
    public Qualification Qualification { get; set; }
    public double TimeSpent { get; set; }
    public DateTime WorklogDate { get; set; }
    public DateTime WorklogStart { get; set; }
    public DateTime RegTimestamp { get; set; }
    public string Descript { get; set; }
    public string Components { get; set; }
}