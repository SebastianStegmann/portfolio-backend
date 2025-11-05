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

    public List<NameSearchResult> FindNames(string name, int personId, int limitCount = 10)
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

    public List<CoplayerSearchResult> FindCoplayers(string nconst, int personId, int limitCount = 10)
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

    // public int SeedNameRatings(List<string> nconsts)
    // {
    //     int successCount = 0;
    //
    //     foreach (var nconst in nconsts)
    //     {
    //         try
    //         {
    //             var result = GetNameRating(nconst);
    //             if (result != null)
    //             {
    //                 successCount++;
    //             }
    //         }
    //         catch (Exception ex)
    //         {
    //             Console.WriteLine($"Error processing nconst {nconst}: {ex.Message}");
    //         }
    //     }
    //
    //     return successCount;
    // }

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

    public List<BestMatchResult> GetBestMatch(List<string> keywords, int limitCount = 10)
    {
        // Convert list to PostgreSQL array format: ARRAY['keyword1','keyword2']
        var keywordsArray = "ARRAY[" + string.Join(",", keywords.Select(k => $"'{k.Replace("'", "''")}'")) + "]";
        // Sanitize input to prevent SQL injection
        var results = _context.Database
            .SqlQueryRaw<BestMatchResult>(
                $"SELECT * FROM best_match_query({keywordsArray}::text[], {limitCount})"
            )
            .ToList();

        return results;
    }

    public List<WordToWordsResult> GetWordToWords(List<string> keywords)
    {
        var keywordsArray = "ARRAY[" + string.Join(",", keywords.Select(k => $"'{k.Replace("'", "''")}'")) + "]";

        var results = _context.Database
            .SqlQueryRaw<WordToWordsResult>(
                $"SELECT * FROM word_to_words_query({keywordsArray}::text[])"
            )
            .ToList();

        return results;
    }


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
