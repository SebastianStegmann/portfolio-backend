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
    public void True()
    {
        Assert.True(true);
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
    public void GetNameProfessions_ReturnsProfessions()
    {
        var np = _context.NameProfessions.FirstOrDefault();
        if (np == null)
        {
            return;
        }

        var result = _service.GetNameProfessions(np.Nconst);

        Assert.NotNull(result);
        Assert.All(result, p => Assert.True(p.Id > 0));
    }

    [Fact]
    public void GetNamesByProfession_ReturnsNames()
    {
        var prof = _context.Professions.FirstOrDefault();
        if (prof == null)
        {
            return;
        }

        var result = _service.GetNamesByProfession(prof.Id);

        Assert.NotNull(result);
        Assert.All(result, n => Assert.False(string.IsNullOrEmpty(n.Nconst)));
    }

    [Fact]
    public void GetNamesKnownForTitle_ReturnsNames()
    {
        var knownFor = _context.KnownFors.FirstOrDefault();
        if (knownFor == null)
        {
            return;
        }

        var result = _service.GetNamesKnownForTitle(knownFor.Tconst);

        Assert.NotNull(result);
        Assert.All(result, n => Assert.False(string.IsNullOrEmpty(n.Nconst)));
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
    public void GetNameRolesInTitle_ReturnsRoles()
    {
        var nameRole = _context.NameTitleRoles.FirstOrDefault();
        if (nameRole == null)
        {
            return;
        }

        var result = _service.GetNameRolesInTitle(nameRole.Nconst, nameRole.Tconst);

        Assert.NotNull(result);
        Assert.All(result, r => Assert.True(r.RoleId > 0));
    }

    [Fact]
    public void GetNamesByRoleInTitle_ReturnsNames()
    {
        var nameRole = _context.NameTitleRoles.FirstOrDefault();
        if (nameRole == null)
        {
            return;
        }

        var result = _service.GetNamesByRoleInTitle(nameRole.Tconst, nameRole.RoleId);

        Assert.NotNull(result);
        Assert.All(result, n => Assert.False(string.IsNullOrEmpty(n.Nconst)));
    }

    [Fact]
    public void GetAllRolesForName_ReturnsRoles()
    {
        var nameRole = _context.NameTitleRoles.FirstOrDefault();
        if (nameRole == null)
        {
            return;
        }

        var result = _service.GetAllRolesForName(nameRole.Nconst);

        Assert.NotNull(result);
        Assert.All(result, r => Assert.True(r.RoleId > 0));
    }

    [Fact]
    public void GetTitlesByNameAndRole_ReturnsTitles()
    {
        var nameRole = _context.NameTitleRoles.FirstOrDefault();
        if (nameRole == null)
        {
            return;
        }

        var result = _service.GetTitlesByNameAndRole(nameRole.Nconst, nameRole.RoleId);

        Assert.NotNull(result);
        Assert.All(result, t => Assert.False(string.IsNullOrEmpty(t.Tconst)));
    }

    [Fact]
    public void GetAllRoles_ReturnsRoles()
    {
        var result = _service.GetAllRoles();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.All(result, r => Assert.True(r.RoleId > 0));
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

