using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using oClock.MVC.Controllers;
using oClock.MVC.Models;
using oClock.MVC.Infrastructure;

namespace oClock.Tests
{
    public class WorklogControllerTests
    {
        private readonly WorklogController _controller;
        private readonly JiraDbContext _context;
        private readonly Mock<IConfiguration> _mockConfiguration;

        public WorklogControllerTests()
        {
            var options = new DbContextOptionsBuilder<JiraDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new JiraDbContext(options);
            SeedData(_context);

            _mockConfiguration = new Mock<IConfiguration>();

            _controller = new WorklogController(_context, _mockConfiguration.Object);
        }

        private void SeedData(JiraDbContext context)
        {
            if (context.Worklogs.Any())
            {
                context.Worklogs.RemoveRange(context.Worklogs);
                context.SaveChanges();
            }

            context.Worklogs.AddRange(new List<Worklog>
            {
                new Worklog 
                { 
                    Ref = 1, 
                    WorklogDate = new DateTime(2024, 1, 1), 
                    Author = "Author1", 
                    TimeSpent = 5, 
                    Qualification = "Dev",
                    Project = "Project1",
                    Issue = "Issue1",
                    IssueSummary = "Issue Summary 1",
                    WorklogStart = DateTime.Now,
                    RegTimestamp = DateTime.Now,
                    Descript = "Description 1",
                    Components = "Component1"
                },
                new Worklog 
                { 
                    Ref = 2, 
                    WorklogDate = new DateTime(2024, 2, 1), 
                    Author = "Author2", 
                    TimeSpent = 8, 
                    Qualification = "QA",
                    Project = "Project2",
                    Issue = "Issue2",
                    IssueSummary = "Issue Summary 2",
                    WorklogStart = DateTime.Now,
                    RegTimestamp = DateTime.Now,
                    Descript = "Description 2",
                    Components = "Component2"
                },
                new Worklog 
                { 
                    Ref = 3, 
                    WorklogDate = new DateTime(2024, 3, 1), 
                    Author = "Author1", 
                    TimeSpent = 3, 
                    Qualification = "Dev",
                    Project = "Project3",
                    Issue = "Issue3",
                    IssueSummary = "Issue Summary 3",
                    WorklogStart = DateTime.Now,
                    RegTimestamp = DateTime.Now,
                    Descript = "Description 3",
                    Components = "Component3"
                }
            });
            context.SaveChanges();
        }

        [Fact]
        public void Index_Returns_ViewResult_With_Data()
        {
            // Act
            var result = _controller.Index(null, null, null) as ViewResult;

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
            Assert.NotNull(viewResult.ViewData);
            Assert.Equal(2, (_controller.ViewBag.TeamData as List<dynamic>).Count);
            Assert.Equal(2, (_controller.ViewBag.Authors as List<string>).Count);
        }

        [Fact]
        public void Index_Returns_Filtered_ViewResult_With_Author()
        {
            // Act
            var result = _controller.Index(null, null, "Author1") as ViewResult;

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
            Assert.NotNull(viewResult.ViewData);
            Assert.Equal(1, (_controller.ViewBag.IndividualData as List<dynamic>).Count);
            Assert.Equal("Author1", _controller.ViewBag.SelectedAuthor);
        }

        [Fact]
        public void Index_Uses_Default_Dates_When_Not_Provided()
        {
            // Act
            var result = _controller.Index(null, null, null) as ViewResult;

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal(new DateTime(DateTime.Now.Year, 1, 1).ToString("yyyy-MM-dd"), _controller.ViewBag.StartDate);
            Assert.Equal(DateTime.Now.ToString("yyyy-MM-dd"), _controller.ViewBag.EndDate);
        }

        [Fact]
        public void Index_Returns_ViewResult_With_Specific_Dates()
        {
            // Arrange
            var startDate = new DateTime(2024, 1, 1);
            var endDate = new DateTime(2024, 12, 31);

            // Act
            var result = _controller.Index(startDate, endDate, null) as ViewResult;

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal(startDate.ToString("yyyy-MM-dd"), _controller.ViewBag.StartDate);
            Assert.Equal(endDate.ToString("yyyy-MM-dd"), _controller.ViewBag.EndDate);
        }
    }
}
