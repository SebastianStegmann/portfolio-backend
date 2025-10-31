using DataServiceLayer.Models.Functions;
using Microsoft.EntityFrameworkCore;

namespace DataServiceLayer;

public class FunctionsDataService : BaseDataService
{
    public FunctionsDataService(ImdbContext context) : base(context)
    {
        Console.WriteLine("FunctionsDataService initialized");
        if (_context == null) Console.WriteLine("Context is null!");
    }

    // Call the find_names stored function from the database ✅
    // Test URL: GET /api/functions/find-names?name=radcliffe&personId=1&limit=10
    public List<NameSearchResult> FindNames(string name, long personId, int limitCount = 10)
    {
        var results = _context.Database
            .SqlQueryRaw<NameSearchResult>(
                "SELECT * FROM find_names({0}, {1}, {2})",
                name,
                personId,
                limitCount
            )
            .ToList();

        return results;
    }

    // Call the find_coplayers stored function from the database ✅
    // Test URL: GET /api/functions/find-coplayers?nconst=nm0705356&personId=2&limit=10
    public List<CoplayerSearchResult> FindCoplayers(string nconst, long personId, int limitCount = 10)
    {
        var results = _context.Database
            .SqlQueryRaw<CoplayerSearchResult>(
                "SELECT * FROM find_coplayers({0}, {1}, {2})",
                nconst,
                personId,
                limitCount
            )
            .ToList();

        return results;
    }

    // Call the namerating stored function from the database ✅
    // Test URL: GET /api/functions/name-rating?nconst=nm0705356
    public NameRatingResult? GetNameRating(string nconst)
    {
        var results = _context.Database
            .SqlQueryRaw<NameRatingResult>(
                "SELECT * FROM namerating({0})",
                nconst
            )
            .ToList();

        return results.FirstOrDefault();
    }

    // Seed name ratings for multiple actors
    // Test URL: POST /api/functions/seed-name-ratings
    // Body: ["nm0000122", "nm0000168", "nm0000313"]
    public int SeedNameRatings(List<string> nconsts)
    {
        int successCount = 0;

        foreach (var nconst in nconsts)
        {
            try
            {
                var result = GetNameRating(nconst);
                if (result != null)
                {
                    successCount++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing nconst {nconst}: {ex.Message}");
            }
        }

        return successCount;
    }

    // Call the popularactors stored function from the database ✅
    // Test URL: GET /api/functions/popular-actors?primaryTitle=Iron man&limit=10
    public List<PopularActorResult> GetPopularActors(string primaryTitle, int limitCount = 10)
    {
        var results = _context.Database
            .SqlQueryRaw<PopularActorResult>(
                "SELECT * FROM popularactors({0}, {1})",
                primaryTitle,
                limitCount
            )
            .ToList();

        return results;
    }

    // Call the popularcoplayers stored function from the database ✅
    // Test URL: GET /api/functions/popular-coplayers?name=Robert Downey Jr.&limit=10
    public List<PopularActorResult> GetPopularCoplayers(string name, int limitCount = 10)
    {
        var results = _context.Database
            .SqlQueryRaw<PopularActorResult>(
                "SELECT * FROM popularcoplayers({0}, {1})",
                name,
                limitCount
            )
            .ToList();

        return results;
    }

    // Call the get_related_movies stored function from the database ✅
    // Test URL: GET /api/functions/related-movies?tconst=tt0371746&limit=10
    public List<RelatedMovieResult> GetRelatedMovies(string tconst, int limitCount = 10)
    {
        var results = _context.Database
            .SqlQueryRaw<RelatedMovieResult>(
                "SELECT * FROM get_related_movies({0}, {1})",
                tconst,
                limitCount
            )
            .ToList();

        return results;
    }

    // Call the freqpersonwords stored function from the database ✅
    // Test URL: GET /api/functions/frequent-person-words?name=Tom Hanks&limit=10
    public List<FrequentWordResult> GetFrequentPersonWords(string name, int limitCount = 10)
    {
        var results = _context.Database
            .SqlQueryRaw<FrequentWordResult>(
                "SELECT * FROM freqpersonwords({0}, {1})",
                name,
                limitCount
            )
            .ToList();

        return results;
    }

    // Call the exactmatch stored function from the database ✅
    // Test URL: GET /api/functions/exact-match?input=iron man&limit=10
    public List<ExactMatchResult> GetExactMatch(string input, int limitCount = 10)
    {
        var results = _context.Database
            .SqlQueryRaw<ExactMatchResult>(
                "SELECT * FROM exactmatch({0}, {1})",
                input,
                limitCount
            )
            .ToList();

        return results;
    }

    // Call the best_match_query stored function from the database ✅
    // Test URL: GET /api/functions/best-match?keywords=iron,man&limit=10
    public List<BestMatchResult> GetBestMatch(List<string> keywords, int limitCount = 10)
    {
        // Convert list to PostgreSQL array format: ARRAY['keyword1','keyword2']
        var keywordsArray = "ARRAY[" + string.Join(",", keywords.Select(k => $"'{k.Replace("'", "''")}'")) + "]";
        // Sanitize input to prevent SQL injection
#pragma warning disable EF1002
        var results = _context.Database
            .SqlQueryRaw<BestMatchResult>(
                $"SELECT * FROM best_match_query({keywordsArray}::text[], {limitCount})"
            )
            .ToList();
#pragma warning restore EF1002

        return results;
    }

    // Call the word_to_words_query stored function from the database
    // Test URL: GET /api/functions/word-to-words?keywords=iron,man
    public List<WordToWordsResult> GetWordToWords(List<string> keywords)
    {
        // Convert list to PostgreSQL array format: ARRAY['keyword1','keyword2']
        var keywordsArray = "ARRAY[" + string.Join(",", keywords.Select(k => $"'{k.Replace("'", "''")}'")) + "]";
        
#pragma warning disable EF1002
        var results = _context.Database
            .SqlQueryRaw<WordToWordsResult>(
                $"SELECT * FROM word_to_words_query({keywordsArray}::text[])"
            )
            .ToList();
#pragma warning restore EF1002
        
        return results;
    }


    // Call the register_person stored function from the database
    // Test URL: POST /api/functions/register-person
    // Body: {"name": "John Doe", "birthday": "1990-01-01", "location": "US", "email": "john@example.com", "password": "hashed_password"}
    public RegisterPersonResult RegisterPerson(string name, DateTime? birthday, string? location, string email, string password)
    {
        var result = _context.Database
            .SqlQueryRaw<RegisterPersonResult>(
                "SELECT * FROM register_person({0}, {1}, {2}, {3}, {4})",
                name,
                birthday,
                location,
                email,
                password
            )
            .ToList()
            .FirstOrDefault();
        
        return result ?? new RegisterPersonResult { Status = "Error: Registration failed", User_Id = null };
    }

    // Call the update_person stored function from the database
    // Test URL: PUT /api/functions/update-person
    // Body: {"userId": 1, "name": "Jane Doe", "birthday": "1990-01-01", "location": "UK", "email": "jane@example.com"}
    public StatusResult UpdatePerson(long userId, string? name, DateTime? birthday, string? location, string? email)
    {
        var result = _context.Database
            .SqlQueryRaw<StatusResult>(
                "SELECT * FROM update_person({0}, {1}, {2}, {3}, {4})",
                userId,
                name,
                birthday,
                location,
                email
            )
            .ToList()
            .FirstOrDefault();
        
        return result ?? new StatusResult { Status = "Error: Update failed" };
    }

    // Call the delete_user stored function from the database
    // Test URL: DELETE /api/functions/delete-user/1
    public StatusResult DeleteUser(long userId)
    {
        var result = _context.Database
            .SqlQueryRaw<StatusResult>(
                "SELECT * FROM delete_user({0})",
                userId
            )
            .ToList()
            .FirstOrDefault();
        
        return result ?? new StatusResult { Status = "Error: Delete failed" };
    }

    // Bookmark Functions

    // Call the add_title_bookmark stored function from the database
    // Test URL: POST /api/functions/bookmarks/title
    // Body: {"userId": 1, "tconst": "tt0371746"}
    public StatusResult AddTitleBookmark(long userId, string tconst)
    {
        var result = _context.Database
            .SqlQueryRaw<StatusResult>(
                "SELECT * FROM add_title_bookmark({0}, {1})",
                userId,
                tconst
            )
            .ToList()
            .FirstOrDefault();
        
        return result ?? new StatusResult { Status = "Error: Bookmark failed" };
    }

    // Call the add_name_bookmark stored function from the database
    // Test URL: POST /api/functions/bookmarks/name
    // Body: {"userId": 1, "nconst": "nm0000375"}
    public StatusResult AddNameBookmark(long userId, string nconst)
    {
        var result = _context.Database
            .SqlQueryRaw<StatusResult>(
                "SELECT * FROM add_name_bookmark({0}, {1})",
                userId,
                nconst
            )
            .ToList()
            .FirstOrDefault();
        
        return result ?? new StatusResult { Status = "Error: Bookmark failed" };
    }

    // Call the get_user_bookmarks stored function from the database
    // Test URL: GET /api/functions/bookmarks/1
    public List<UserBookmarkResult> GetUserBookmarks(long userId)
    {
        var results = _context.Database
            .SqlQueryRaw<UserBookmarkResult>(
                "SELECT * FROM get_user_bookmarks({0})",
                userId
            )
            .ToList();
        
        return results;
    }

    // Call the delete_title_bookmark stored function from the database
    // Test URL: DELETE /api/functions/bookmarks/title
    // Body: {"userId": 1, "tconst": "tt0371746"}
    public StatusResult DeleteTitleBookmark(long userId, string tconst)
    {
        var result = _context.Database
            .SqlQueryRaw<StatusResult>(
                "SELECT * FROM delete_title_bookmark({0}, {1})",
                userId,
                tconst
            )
            .ToList()
            .FirstOrDefault();
        
        return result ?? new StatusResult { Status = "Error: Delete bookmark failed" };
    }

    // Call the delete_name_bookmark stored function from the database
    // Test URL: DELETE /api/functions/bookmarks/name
    // Body: {"userId": 1, "nconst": "nm0000375"}
    public StatusResult DeleteNameBookmark(long userId, string nconst)
    {
        var result = _context.Database
            .SqlQueryRaw<StatusResult>(
                "SELECT * FROM delete_name_bookmark({0}, {1})",
                userId,
                nconst
            )
            .ToList()
            .FirstOrDefault();
        
        return result ?? new StatusResult { Status = "Error: Delete bookmark failed" };
    }

    // Call the string_search stored function from the database ✅
    // Test URL: GET /api/functions/string-search?searchString=batman&personId=1
    public List<StringSearchResult> StringSearch(string searchString, long personId)
    {
        var results = _context.Database
            .SqlQueryRaw<StringSearchResult>(
                "SELECT * FROM string_search({0}, {1})",
                searchString,
                personId
            )
            .ToList();
        
        return results;
    }

    // Call the rate stored function from the database
    // Test URL: POST /api/functions/rate
    // Body: {"tconst": "tt0371746", "personId": 1, "rating": 8}
    public StatusResult RateTitle(string tconst, long personId, int rating)
    {
        var result = _context.Database
            .SqlQueryRaw<StatusResult>(
                "SELECT * FROM rate({0}, {1}, {2})",
                tconst,
                personId,
                rating
            )
            .ToList()
            .FirstOrDefault();
        
        return result ?? new StatusResult { Status = "Error: Rating failed" };
    }

    // Call the structured_string_search stored function from the database
    // Test URL: POST /api/functions/structured-search
    // Body: {"titleText": "Iron", "plotText": "superhero", "characterText": "Tony Stark", "personText": "Robert Downey", "personId": 1, "limit": 10}
    public List<StringSearchResult> StructuredStringSearch(
        string? titleText, 
        string? plotText, 
        string? characterText, 
        string? personText, 
        long personId, 
        int limitCount = 10)
    {
        var results = _context.Database
            .SqlQueryRaw<StringSearchResult>(
                "SELECT * FROM structured_string_search({0}, {1}, {2}, {3}, {4}, {5})",
                titleText,
                plotText,
                characterText,
                personText,
                personId,
                limitCount
            )
            .ToList();
        
        return results;
    }
}
