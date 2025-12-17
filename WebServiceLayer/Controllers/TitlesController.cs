using DataServiceLayer;
using DataServiceLayer.Models;
using DataServiceLayer.Models.Title;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using WebServiceLayer.Models;
using WebServiceLayer.Models.DTO;

namespace WebServiceLayer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TitlesController : BaseController<TitleDataService>
{
  public TitlesController(
      TitleDataService dataService,
      LinkGenerator generator,
      IMapper mapper) : base(dataService, generator, mapper) { }

  // Getting all titles - GET: api/titles
  [HttpGet(Name = nameof(GetTitles))]
  public IActionResult GetTitles([FromQuery] QueryParams queryParams)
  {
    try
    {
      var titles = _dataService
          .GetTitles(queryParams.Page, queryParams.PageSize)
          .Select(x => CreateTitleListModel(x));

      var numOfItems = _dataService.GetTitlesCount();

      var result = CreatePaging(nameof(GetTitles), titles, numOfItems, queryParams);

      return Ok(result);
    }
    catch (Exception)
    {
      return StatusCode(500, "An error occurred while retrieving titles");
    }
  }

  // Getting one title by tconst - GET: api/titles/{tconst}
  [HttpGet("{tconst}", Name = nameof(GetTitle))]
  public IActionResult GetTitle(string tconst)
  {
    try
    {
      var title = _dataService.GetTitle(tconst);
      if (title == null) return NotFound();
      TitleModel model = CreateTitleModel(title);

      return Ok(model);
    }
    catch (Exception)
    {
      return StatusCode(500, "An error occurred while retrieving the title");
    }
  }

  // api/titles/{tconst}/genres
  [HttpGet("{tconst}/genres", Name = nameof(GetGenresForTitle))]
  public IActionResult GetGenresForTitle(string tconst)
  {
    try
    {
      var title = _dataService.GetTitle(tconst);
      if (title == null) return NotFound();

      var genres = title.Genre
          .Where(tg => tg.Genre != null)
          .Select(tg => new GenreDTO
          {
              GenreName = tg.Genre!.GenreName
          })
          .ToList();

      if (genres.Count == 0) return NotFound();
      return Ok(genres);
    }
    catch (Exception)
    {
      return StatusCode(500, "An error occurred while retrieving genres");
    }
  }

  // api/titles/{tconst}/akas
  [HttpGet("{tconst}/akas", Name = nameof(GetAkasForTitle))]
  public IActionResult GetAkasForTitle(string tconst)
  {
    try
    {
      var title = _dataService.GetTitle(tconst);
      if (title == null) return NotFound();

      var akas = title.Aka
          .Select(aka => new AkaDTO
          {
              Title = aka.Title,
              Region = aka.Region,
              Language = aka.Language
          })
          .ToList();

      if (akas.Count == 0) return NotFound();
      return Ok(akas);
    }
    catch (Exception)
    {
      return StatusCode(500, "An error occurred while retrieving alternate titles");
    }
  }

  // api/titles/{tconst}/episodes
  [HttpGet("{tconst}/episodes", Name = nameof(GetEpisodesForTitle))]
  public IActionResult GetEpisodesForTitle(string tconst)
  {
    try
    {
      var title = _dataService.GetTitle(tconst);
      if (title == null) return NotFound();

      var episodes = title.Episodes
          .Select(ep => new EpisodeDTO
          {
              URL = GetUrl("GetTitle", new { Tconst = ep.Tconst.Trim() }),
              PrimaryTitle = ep.PrimaryTitle,
              SeasonNumber = ep.SeasonNumber,
              EpisodeNumber = ep.EpisodeNumber,
              ReleaseDate = ep.ReleaseDate
          })
          .OrderBy(ep => ep.SeasonNumber)
          .ThenBy(ep => ep.EpisodeNumber)
          .ToList();

      if (episodes.Count == 0) return NotFound();
      return Ok(episodes);
    }
    catch (Exception)
    {
      return StatusCode(500, "An error occurred while retrieving episodes");
    }
  }

  // api/titles/{tconst}/allcast
  [HttpGet("{tconst}/allcast", Name = nameof(GetCastForTitle))]
  public IActionResult GetCastForTitle(string tconst)
  {
    try
    {
      var cast = _dataService.GetCastForTitle(tconst);
      if (cast == null || cast.Count == 0) return NotFound();

      var castModel = cast.Select(castName => new CastMemberModel
      {
          URL = GetUrl("GetName", new { Nconst = castName.Nconst.Trim() }),
          Name = castName.Name,
          Category = castName.Category,
          Characters = castName.Characters,
          Job = castName.Job,

          KnownForURL = _dataService.HasKnownForTitles(castName.Nconst.Trim())
          ? GetUrl("GetKnownForTitles", new { nconst = castName.Nconst.Trim() })
          : null
      });

      return Ok(castModel);
    }
    catch (Exception)
    {
      return StatusCode(500, "An error occurred while retrieving cast");
    }
  }

  // awards endpoint
  [HttpGet("{tconst}/awards", Name = nameof(GetAwardsByTitle))]
  public IActionResult GetAwardsByTitle(string tconst)
  {
    try
    {
      var awards = _dataService.GetAwardsByTitle(tconst);
      return Ok(awards);
    }
    catch (Exception)
    {
      return StatusCode(500, "An error occurred while retrieving awards");
    }
  }

  // api/titles/{tconst}/overallrating
  [HttpGet("{tconst}/overallrating", Name = nameof(GetOverallRatings))]
  public IActionResult GetOverallRatings(string tconst)
  {
    try
    {
      var overallRating = _dataService.GetOverallRatings(tconst);
      if (overallRating == null) return NotFound();

      var ratings = new RatingDTO
      {
          Rating = overallRating.Rating, 
          Votes = overallRating.Votes 
      };

      return Ok(ratings);
    }
    catch (Exception)
    {
      return StatusCode(500, "An error occurred while retrieving ratings");
    }
  }
    //object-object mapping

    private TitleListModel CreateTitleListModel(DataServiceLayer.Models.Title.TitleBasics title)
    {
        var model = _mapper.Map<TitleListModel>(title);
        model.URL = GetUrl(nameof(GetTitle), new { Tconst = title.Tconst.Trim() });

        if (title.Names != null && title.Names.Any())
        { 
            model.AllCastURL = GetUrl(nameof(GetCastForTitle), new { tconst = title.Tconst.Trim() });
        }

            return model;
    }

    // Information shown when clicking on a specific title
    private TitleModel CreateTitleModel(DataServiceLayer.Models.Title.TitleBasics title)
    {
        var model = _mapper.Map<TitleModel>(title);
        model.URL = GetUrl(nameof(GetTitle), new { Tconst = title.Tconst.Trim() });

        if (title.Names != null && title.Names.Any())
        {
            model.AllCastURL = GetUrl(nameof(GetCastForTitle), new { tconst = title.Tconst.Trim() });
        }
        else
        {
            model.AllCastURL = null;
        }

        // Generate GenresURL if genres exist
        if (title.Genre != null && title.Genre.Any())
        {
            model.GenresURL = GetUrl(nameof(GetGenresForTitle), new { tconst = title.Tconst.Trim() });
        }

        // Generate AlternateTitlesURL if alternate titles exist
        if (title.Aka != null && title.Aka.Any())
        {
            model.AkaURL = GetUrl(nameof(GetAkasForTitle), new { tconst = title.Tconst.Trim() });
        }

        // Generate EpisodesURL if episodes exist
        if (title.Episodes != null && title.Episodes.Any())
        {
            model.EpisodesURL = GetUrl(nameof(GetEpisodesForTitle), new { tconst = title.Tconst.Trim() });
        }

        // Map OverallRating as a nested object (if it exists)
        if (title.OverallRating != null)
        {
            model.OverallRating = new RatingDTO
            {
                Rating = title.OverallRating.Rating,
                Votes = title.OverallRating.Votes
            };
        }

        // Map Award information (if it exists)
        if (title.Award != null)
        {
            model.Awards = title.Award.AwardInfo;
        }

        return model;
    }
}
