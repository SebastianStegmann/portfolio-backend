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
    protected (JObject?, HttpStatusCode) PostData(string url, object content, string? jwt = null)
    {
        using var client = new HttpClient();
        if (!string.IsNullOrEmpty(jwt))
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);
        }

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
            Assert.Fail($"Unexpected non-JSON response from {url}: {dataStr}"); // Fail the test
            return (null, response.StatusCode);
        }
    }

    protected (JArray?, HttpStatusCode) GetArray(string url, string? jwt = null)
    {
        using var client = new HttpClient();

        if (!string.IsNullOrEmpty(jwt))
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);
        }
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

    protected (JObject?, HttpStatusCode) GetObject(string url, string? jwt = null)
    {
        using var client = new HttpClient();

        if (!string.IsNullOrEmpty(jwt))
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);
        }

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
    }

    [Fact]
    public void ApiTitles_GetTitle_ReturnsOkAndNonEmpty()
    {
        var tconst = "tt0167261"; // LOTR
        var (data, statusCode) = GetObject($"{TitlesApi}/{tconst}");
        Assert.Equal(HttpStatusCode.OK, statusCode);
        Assert.NotNull(data);
        Assert.NotEmpty(data);
    }

    [Fact]
    public void ApiTitles_GetTitleGenres()
    {
        var tconst = "tt0167261"; // LOTR
        var (data, statusCode) = GetArray($"{TitlesApi}/{tconst}/genres");
        Assert.Equal(HttpStatusCode.OK, statusCode);
        Assert.NotNull(data);
        Assert.NotEmpty(data);
    }

    [Fact]
    public void ApiTitles_GetTitleAkas()
    {
        var tconst = "tt0167261"; // LOTR
        var (data, statusCode) = GetArray($"{TitlesApi}/{tconst}/akas");
        Assert.Equal(HttpStatusCode.OK, statusCode);
        Assert.NotNull(data);
        Assert.NotEmpty(data);
    }

    [Fact]
    public void ApiTitles_GetTitleEpisodes()
    {
        var tconst = "tt0098936"; // Twin Peaks
        var (data, statusCode) = GetArray($"{TitlesApi}/{tconst}/episodes");
        Assert.Equal(HttpStatusCode.OK, statusCode);
        Assert.NotNull(data);
        Assert.NotEmpty(data);
    }

    [Fact]
    public void ApiTitles_GetTitleAllCast()
    {
        var tconst = "tt0098936"; // Twin Peaks
        var (data, statusCode) = GetArray($"{TitlesApi}/{tconst}/allcast");
        Assert.Equal(HttpStatusCode.OK, statusCode);
        Assert.NotNull(data);
        Assert.NotEmpty(data);
    }

    [Fact]
    public void ApiTitles_GetTitleAwards()
    {
        var tconst = "tt0098936"; // Twin Peaks
        var (data, statusCode) = GetObject($"{TitlesApi}/{tconst}/awards");
        Assert.Equal(HttpStatusCode.OK, statusCode);
        Assert.NotNull(data);
        Assert.NotEmpty(data);
    }

    [Fact]
    public void ApiTitles_GetTitleOverallRating()
    {
        var tconst = "tt0098936"; // Twin Peaks
        var (data, statusCode) = GetObject($"{TitlesApi}/{tconst}/overallrating");
        Assert.Equal(HttpStatusCode.OK, statusCode);
        Assert.NotNull(data);
        Assert.NotEmpty(data);
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
        output.WriteLine(loginData?.ToString() ?? "The login data was null (No JWT returned)");
        output.WriteLine($"Login: {loginStatus}");

        // Grab JWT for future tests
        JWT = loginData?["token"]?.Value<string>();
        Assert.NotNull(JWT);

        // Test using no JWT (expect Unauthorized for protected endpoint)
        var (noJwtData, noJwtStatus) = GetObject(PersonApi, null);
        Assert.Equal(HttpStatusCode.Unauthorized, noJwtStatus);
        output.WriteLine($"No JWT: {noJwtStatus}");

        // Test using bad/invalid JWT (expect Unauthorized)
        var invalidJwt = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.invalid.payload.invalid-signature"; // Bad token
        var (badJwtData, badJwtStatus) = GetObject(PersonApi, invalidJwt);
        Assert.Equal(HttpStatusCode.Unauthorized, badJwtStatus); 
        output.WriteLine($"Bad JWT: {badJwtStatus}");

        // Test using valid JWT for same user (expect OK)
        var (validJwtData, validJwtStatus) = GetObject(PersonApi, JWT);
        Assert.Equal(HttpStatusCode.OK, validJwtStatus);
        Assert.NotNull(validJwtData);
        output.WriteLine($"Valid JWT: {validJwtStatus}");

        // Delete
        var (delData, delStatus) = PostData($"{AuthApi}/delete", _userData);
        output.WriteLine(delData?.ToString() ?? "The returned delete data was null");
        output.WriteLine($"Delete: {delStatus}");
        Assert.Equal(HttpStatusCode.OK, delStatus);
    }

}
