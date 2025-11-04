using Xunit;
using DataServiceLayer;
using DataServiceLayer.Models.Title;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace DataService.Tests;

public class TitleDataServiceTests
{

    private readonly TitleDataService _service;
    private readonly ImdbContext _context;

    public TitleDataServiceTests()
    {
        var projectRoot = Directory.GetParent(AppContext.BaseDirectory)
                           !.Parent
                           !.Parent // bin/
                           !.Parent // Debug/
                           !.Parent // net10.0/
                           !.Parent // DataService.Tests/
                           !.FullName;

        var jsonPath = Path.Combine(projectRoot, "WebServiceLayer", "config.json");
        var jsonContent = File.ReadAllText(jsonPath); 
        using var doc = JsonDocument.Parse(jsonContent);

        var connectionString = doc.RootElement.GetProperty("ConnectionString").GetString();
        // var fullPath = Path.GetFullPath(jsonPath);
        // Console.WriteLine($"JSON Path: {fullPath}");

        var options = new DbContextOptionsBuilder<ImdbContext>()
            .UseNpgsql(connectionString)
            .Options;

        _context = new ImdbContext(options);
        _service = new TitleDataService(_context);
    }

    [Fact]
    public void True()
    {
        Assert.True(true);
    }


    [Fact]
    public void False()
    {
        Assert.False(false);
    }

    [Fact]
    public void GetTitlesCount_NoArgument_ReturnsAllTitles()
    {
        var count = _service.GetTitlesCount();
        Assert.True(count == 66550);
    }

    [Fact]
        public void GetTitles_ReturnsPagedResults()
        {
            int page = 0;
            int pageSize = 10;

            var result = _service.GetTitles(page, pageSize);

            Assert.NotNull(result);
            Assert.True(result.Count <= pageSize);
            Assert.All(result, t => Assert.NotNull(t.Tconst));
        }

        [Fact]
        public void GetTitle_ValidTconst_ReturnsTitle()
        {
            var firstTitle = _context.TitleBasics.FirstOrDefault();
            Assert.NotNull(firstTitle);

            var result = _service.GetTitle(firstTitle.Tconst);

            Assert.NotNull(result);
            Assert.Equal(firstTitle.Tconst, result!.Tconst);
            Assert.NotNull(result.Names);
        }

        [Fact]
        public void GetTitle_InvalidTconst_ReturnsNull()
        {
            string fakeTconst = "tt0000000";

            var result = _service.GetTitle(fakeTconst);

            Assert.Null(result);
        }

        [Fact]
        public void GetAllGenres_ReturnsGenres()
        {
            var result = _service.GetAllGenres();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.All(result, g => Assert.False(string.IsNullOrEmpty(g.GenreName)));
        }

        [Fact]
        public void GetCastForTitle_ReturnsCastMembers()
        {
            var titleWithCast = _context.TitlePrincipals.FirstOrDefault();
            Assert.NotNull(titleWithCast);

            var result = _service.GetCastForTitle(titleWithCast.Tconst);

            Assert.NotNull(result);
            Assert.All(result, cm =>
            {
                Assert.False(string.IsNullOrEmpty(cm.Nconst));
                Assert.False(string.IsNullOrEmpty(cm.Name));
            });
        }

        [Fact]
        public void GetAwardsByTconst_ReturnsAwards()
        {
            var titleWithAwards = _context.Awards.FirstOrDefault();
            if (titleWithAwards == null)
            {
                return;
            }

            var result = _service.GetAwardsByTconst(titleWithAwards.Tconst);

            Assert.NotNull(result);
            Assert.All(result, a => Assert.Equal(titleWithAwards.Tconst, a.Tconst));
        }

        [Fact]
        public void GetOverallRatings_ReturnsRatings()
        {
            var ratedTitle = _context.OverallRatings.FirstOrDefault();
            if (ratedTitle == null)
            {
                return;
            }

            var result = _service.GetOverallRatings(ratedTitle.Tconst);

            Assert.NotNull(result);
            Assert.All(result, r => Assert.Equal(ratedTitle.Tconst, r.Tconst));
        }

}
