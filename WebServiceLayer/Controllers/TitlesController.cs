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
    var titles = _dataService
        .GetTitles(queryParams.Page, queryParams.PageSize)
        .Select(x => CreateTitleListModel(x));

    var numOfItems = _dataService.GetTitlesCount();

    var result = CreatePaging(nameof(GetTitles), titles, numOfItems, queryParams);

    return Ok(result);
  }

  // Getting one title by tconst - GET: api/titles/{tconst}
  [HttpGet("{tconst}", Name = nameof(GetTitle))]
  public IActionResult GetTitle(string tconst)
  {
    var title = _dataService.GetTitle(tconst);
    if (title == null) return NotFound();
    TitleModel model = CreateTitleModel(title);

    return Ok(model);
  }

    // Getting the genres for the title - GET: api/titles/{tconst}/genres
    [HttpGet("{tconst}/genres", Name = nameof(GetGenresForTitle))]
    public IActionResult GetGenresForTitle(string tconst)
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

    // Getting the alternate titles for the movie - GET: api/titles/{tconst}/akas
    [HttpGet("{tconst}/akas", Name = nameof(GetAkasForTitle))]
    public IActionResult GetAkasForTitle(string tconst)
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

    // Getting the episodes for series - GET: api/titles/{tconst}/episodes
    [HttpGet("{tconst}/episodes", Name = nameof(GetEpisodesForTitle))]
    public IActionResult GetEpisodesForTitle(string tconst)
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
            .ToList();

        if (episodes.Count == 0) return NotFound();
        return Ok(episodes);
    }

    // Getting all names known for a specific title - GET: api/titles/{tconst}/allcast
    [HttpGet("{tconst}/allcast", Name = nameof(GetCastForTitle))]
    public IActionResult GetCastForTitle(string tconst)
    {
        var cast = _dataService.GetCastForTitle(tconst);
        if (cast == null || cast.Count == 0) return NotFound();

        //Map tp NameListModel with URLs
        var castModel = cast.Select(castName => new NameListModel
        {
            URL = GetUrl("GetName", new { Nconst = castName.Nconst.Trim() }),
            Name = castName.Name,
            Category = castName.Category,
            Characters = castName.Characters,
            Job = castName.Job,
            // Only generate KnownForURL if the actor has known for titles
            KnownForURL = _dataService.HasKnownForTitles(castName.Nconst.Trim())
            ? GetUrl("GetKnownForTitles", new { nconst = castName.Nconst.Trim() })
            : null
        });

        return Ok(castModel);
    }

    // awards endpoint
    [HttpGet("{tconst}/awards")]
  public IActionResult GetAwardsByTconst(string tconst)
  {
    var awards = _dataService.GetAwardsByTconst(tconst);
    return Ok(awards);
  }

  // overall rating endpoint
  [HttpGet("{tconst}/overallrating")]
  public IActionResult GetOverallRatings(string tconst)
  {
    var overallRatings = _dataService.GetOverallRatings(tconst);
    return Ok(overallRatings);
  }

    //object-object mapping

    // Information shown when listing all the titles
    private TitleListModel CreateTitleListModel(DataServiceLayer.Models.Title.TitleBasics title)
    {
        var model = _mapper.Map<TitleListModel>(title);
        model.URL = GetUrl(nameof(GetTitle), new { Tconst = title.Tconst.Trim() });

        // Only generate AllCastURL if the movie has registered actors
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

        // mapping of genres to only show genre names
        model.Genres = title.Genre
            .Where(tg => tg.Genre != null)
            .Select(tg => new GenreDTO
            {
                GenreName = tg.Genre!.GenreName
            })
            .ToList();

        // mapping of aka for a simplified view
        model.Akas = title.Aka
        .Select(aka => new AkaDTO
        {
            Title = aka.Title,
            Region = aka.Region,
            Language = aka.Language
        })
        .ToList();

        // mapping of episodes for a simplified view
        model.Episodes = title.Episodes
        .Select(ep => new EpisodeDTO
        {
            URL = GetUrl("GetTitle", new { Tconst = ep.Tconst.Trim() }),
            PrimaryTitle = ep.PrimaryTitle,
            SeasonNumber = ep.SeasonNumber,
            EpisodeNumber = ep.EpisodeNumber,
            ReleaseDate = ep.ReleaseDate
        })
        .ToList();

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

        return model;
    }
}
