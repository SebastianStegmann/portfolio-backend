using Xunit;
using DataServiceLayer;
using DataServiceLayer.Models.Name;
using DataServiceLayer.Models.Title;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace DataService.Tests;

public class NameDataServiceTests
{
    private readonly NameDataService _service;
    private readonly ImdbContext _context;

    public NameDataServiceTests()
    {
        // Navigate back to the project root to find config.json
        var projectRoot = Directory.GetParent(AppContext.BaseDirectory)!
            .Parent!.Parent!.Parent!.Parent!.Parent!
            .FullName;

        var jsonPath = Path.Combine(projectRoot, "WebServiceLayer", "config.json");
        var jsonContent = File.ReadAllText(jsonPath);
        using var doc = JsonDocument.Parse(jsonContent);

        var connectionString = doc.RootElement.GetProperty("ConnectionString").GetString();

        var options = new DbContextOptionsBuilder<ImdbContext>()
            .UseNpgsql(connectionString)
            .Options;

        _context = new ImdbContext(options);
        _service = new NameDataService(_context);
    }

    [Fact]
    public void GetNamesCount_ReturnsCount()
    {
        var count = _service.GetNamesCount();
        Assert.True(count > 0);
    }

    [Fact]
    public void GetNames_ReturnsPagedResults()
    {
        int page = 0;
        int pageSize = 10;

        var result = _service.GetNames(page, pageSize);

        Assert.NotNull(result);
        Assert.True(result.Count <= pageSize);
        Assert.All(result, n => Assert.False(string.IsNullOrEmpty(n.Nconst)));
    }

    [Fact]
    public void GetName_ValidNconst_ReturnsName()
    {
        var firstName = _context.NameBasics.FirstOrDefault();
        Assert.NotNull(firstName);

        var result = _service.GetName(firstName.Nconst);

        Assert.NotNull(result);
        Assert.Equal(firstName.Nconst, result!.Nconst);
        Assert.NotNull(result.Titles);
    }

    [Fact]
    public void GetName_InvalidNconst_ReturnsNull()
    {
        var result = _service.GetName("nm0000000");
        Assert.Null(result);
    }

    [Fact]
    public void GetKnownForTitles_ReturnsTitles()
    {
        var knownFor = _context.KnownFors.FirstOrDefault();
        if (knownFor == null)
        {
            return;
        }

        var result = _service.GetKnownForTitles(knownFor.Nconst);

        Assert.NotNull(result);
        Assert.All(result, t => Assert.False(string.IsNullOrEmpty(t.Tconst)));
    }

    [Fact]
    public void GetAllProfessions_ReturnsList()
    {
        var result = _service.GetAllProfessions();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.All(result, p => Assert.True(p.Id > 0));
    }

    [Fact]
    public void HasKnownForTitles_ReturnsBoolean()
    {
        var knownFor = _context.KnownFors.FirstOrDefault();
        if (knownFor == null)
        {
            return;
        }

        var result = _service.HasKnownForTitles(knownFor.Nconst);

        Assert.True(result);
    }
}

