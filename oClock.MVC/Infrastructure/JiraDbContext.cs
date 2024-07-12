using Microsoft.EntityFrameworkCore;
using oClock.MVC.Models;

namespace oClock.MVC.Infrastructure;

public class JiraDbContext : DbContext
{
    public JiraDbContext(DbContextOptions<JiraDbContext> options)
        : base(options)
    {
    }

    public DbSet<Worklog> Worklogs { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Worklog>(entity =>
        {
            entity.ToTable("X_JIRA_WORKLOG");
            
            entity.Property(e => e.Ref).HasColumnName("REF");
            entity.Property(e => e.Author).HasColumnName("AUTHOR").HasMaxLength(1024);
            entity.Property(e => e.Project).HasColumnName("PROJECT").HasMaxLength(1024);
            entity.Property(e => e.Issue).HasColumnName("ISSUE").HasMaxLength(1024);
            entity.Property(e => e.IssueSummary).HasColumnName("ISSUESUMMARY").HasMaxLength(1024);
            entity.Property(e => e.Qualification).HasColumnName("QUALIFICATION").HasMaxLength(1024);;
            entity.Property(e => e.TimeSpent).HasColumnName("TIMESPENT").HasColumnType("DOUBLE PRECISION");
            entity.Property(e => e.WorklogDate).HasColumnName("WORKLOGDATE").HasColumnType("DATE");
            entity.Property(e => e.WorklogStart).HasColumnName("WORKLOGSTART").HasColumnType("TIMESTAMP");
            entity.Property(e => e.RegTimestamp).HasColumnName("REGTIMESTAMP").HasColumnType("TIMESTAMP");
            entity.Property(e => e.Descript).HasColumnName("DESCRIPT").HasMaxLength(10240);
            entity.Property(e => e.Components).HasColumnName("COMPONENTS").HasMaxLength(1024);
        });
    }
}