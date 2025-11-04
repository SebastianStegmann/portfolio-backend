using System;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace DataService.IntegrationTests;

public abstract class ApiTestBase
{
    protected readonly ITestOutputHelper output;

    public ApiTestBase(ITestOutputHelper output)
    {
        this.output = output;
    }

    // Helpers
    protected (JObject?, HttpStatusCode) PostData(string url, object content)
    {
        using var client = new HttpClient();
        var requestContent = new StringContent(
            JsonConvert.SerializeObject(content),
            Encoding.UTF8,
            "application/json");
        var response = client.PostAsync(url, requestContent).Result;
        var dataStr = response.Content.ReadAsStringAsync().Result;
        if (!response.IsSuccessStatusCode)
        {
            output.WriteLine($"POST {url} failed with {response.StatusCode}: {dataStr}");
            return (null, response.StatusCode);
        }
        try
        {
            var json = JsonConvert.DeserializeObject<JObject>(dataStr);
            return (json, response.StatusCode);
        }
        catch (JsonException ex)
        {
            output.WriteLine($"POST {url} succeeded but invalid JSON: {dataStr}. Error: {ex.Message}");
            return (null, response.StatusCode);
        }
    }

    protected (JArray?, HttpStatusCode) GetArray(string url)
    {
        using var client = new HttpClient();
        var response = client.GetAsync(url).Result;
        var dataStr = response.Content.ReadAsStringAsync().Result;
        if (!response.IsSuccessStatusCode)
        {
            output.WriteLine($"GET {url} failed with {response.StatusCode}: {dataStr}");
            return (null, response.StatusCode);
        }
        try
        {
            var json = JsonConvert.DeserializeObject<JArray>(dataStr);
            return (json, response.StatusCode);
        }
        catch (JsonException ex)
        {
            output.WriteLine($"GET {url} succeeded but invalid JSON: {dataStr}. Error: {ex.Message}");
            return (null, response.StatusCode);
        }
    }

    protected (JObject?, HttpStatusCode) GetObject(string url)
    {
        using var client = new HttpClient();
        var response = client.GetAsync(url).Result;
        var dataStr = response.Content.ReadAsStringAsync().Result;
        if (!response.IsSuccessStatusCode)
        {
            output.WriteLine($"GET {url} failed with {response.StatusCode}: {dataStr}");
            return (null, response.StatusCode);
        }
        try
        {
            var json = JsonConvert.DeserializeObject<JObject>(dataStr);
            return (json, response.StatusCode);
        }
        catch (JsonException ex)
        {
            output.WriteLine($"GET {url} succeeded but invalid JSON: {dataStr}. Error: {ex.Message}");
            return (null, response.StatusCode);
        }
    }
}

public class NameApiTests : ApiTestBase
{
    private const string NamesApi = "http://localhost:5113/api/names";

    public NameApiTests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void ApiNames_GetAll_ReturnsOkAndNonEmpty()
    {
        var (data, statusCode) = GetObject(NamesApi);
        Assert.Equal(HttpStatusCode.OK, statusCode);
        Assert.NotNull(data);
        Assert.NotEmpty(data);
        // Assert.NotNull(data[0]["nconst"]);
    }

    [Fact]
    public void ApiNames_GetByValidNconst_ReturnsName()
    {
        var nconst = "nm0000154"; // Mel Gibson
        var (name, statusCode) = GetObject($"{NamesApi}/{nconst}");
        Assert.Equal(HttpStatusCode.OK, statusCode);
        Assert.NotNull(name);
        Assert.EndsWith(nconst, name["url"]?.ToString());
        Assert.Equal("Mel Gibson", name["name"]?.ToString());
    }

    [Fact]
    public void ApiNames_GetByInvalidNconst_ReturnsNotFound()
    {
        var nconst = "nm0000000"; // Doesn't exist
        var (_, statusCode) = GetObject($"{NamesApi}/{nconst}");
        Assert.Equal(HttpStatusCode.NotFound, statusCode);
    }

    [Fact]
    public void ApiNames_GetByProfession_ReturnsNames()
    {
        var professionId = 1;
        var (names, statusCode) = GetArray($"{NamesApi}/byprofession/{professionId}");
        Assert.Equal(HttpStatusCode.OK, statusCode);
        Assert.NotNull(names);
        Assert.NotEmpty(names);
        Assert.All(names, n => Assert.NotNull(n["nconst"]));
    }
}

// #################################
public class TitleApiTests : ApiTestBase
{
    private const string TitlesApi = "http://localhost:5113/api/titles";

    public TitleApiTests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void ApiTitles_GetAll_ReturnsOkAndNonEmpty()
    {
        var (data, statusCode) = GetObject(TitlesApi);
        Assert.Equal(HttpStatusCode.OK, statusCode);
        Assert.NotNull(data);
        Assert.NotEmpty(data);
        // Assert.NotNull(data[0]["nconst"]);
    }
}

public class AuthApiTests : ApiTestBase
{
    private const string PersonApi = "http://localhost:5113/api/Person";
    private const string AuthApi = "http://localhost:5113/api/Auth";
    public static string? JWT { get; set; }

    private readonly object _userData;

    public AuthApiTests(ITestOutputHelper output) : base(output)
    {
        _userData = new
        {
            Email = $"testuser@test.com",
            Password = "TestPassword123!"
        };
    }

[Fact]
public void ApiAuth_FullFlow()
{
    // Register
    var (regData, regStatus) = PostData($"{AuthApi}/register", _userData);
    Assert.Equal(HttpStatusCode.OK, regStatus);
    output.WriteLine($"Register: {regStatus}");

    // Login
    var (loginData, loginStatus) = PostData($"{AuthApi}/login", _userData);
    Assert.Equal(HttpStatusCode.OK, loginStatus);
    output.WriteLine(loginData?.ToString() ?? "login data was null");
    output.WriteLine($"Login: {loginStatus}");
    // Optionally extract JWT if present
    JWT = loginData?["token"]?.Value<string>();
    Assert.NotNull(JWT);

    //Test using not using JWT
    //
    //Test using JWT but for wrong user id
    //
    //Test using JWT for same user

    // Delete
    var (delData, delStatus) = PostData($"{AuthApi}/delete", _userData);
    output.WriteLine(delData?.ToString() ?? "delete data was null");
    output.WriteLine($"Delete: {delStatus}");
    Assert.Equal(HttpStatusCode.OK, delStatus);
}




    // BRUG SOFIES NYE KODE
    // [Fact]
    // public void API_GetBookmarksWithoutJWT()
    // {
    //     var (data, statusCode) = GetObject($"{PersonApi}/");
    //     Assert.Equal(HttpStatusCode.OK, statusCode);
    //     Assert.NotNull(data);
    //     Assert.NotEmpty(data);
    //     // Assert.NotNull(data[0]["nconst"]);
    // }
    //




}
