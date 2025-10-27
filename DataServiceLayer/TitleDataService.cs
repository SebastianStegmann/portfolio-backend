using DataServiceLayer.Models;
using DataServiceLayer.Models.NameBasics;
using DataServiceLayer.Models.TitleBasics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace DataServiceLayer;

public class TitleDataService : BaseDataService
{
  public TitleDataService(ImdbContext context) : base(context) { }

  //Title

  public int GetTitlesCount()
  {
    return _context.TitleBasics.Count();
  }

  public List<TitleBasics> GetTitles(int page, int pageSize)
  {
    return _context.TitleBasics
        .Include(x => x.Names)
        .OrderBy(x => x.Tconst)
        .Skip(page * pageSize)
        .Take(pageSize)
        .ToList();
  }

  public TitleBasics? GetTitle(string tconst)
  {
    return _context.TitleBasics
            .Include(t => t.Names)
            .FirstOrDefault(t => t.Tconst == tconst);
    }

  public List<Genre> GetAllGenres()
  {
    return _context.Genres.ToList();
  }

  // Get all actors (names) known for a specific movie
  public List<NameBasics> GetActorsForTitle(string tconst)
  {
    return _context.KnownFors
      .Where(kf => kf.Tconst == tconst)
      .Join(_context.NameBasics,
          kf => kf.Nconst,
          nb => nb.Nconst,
          (kf, nb) => nb)
      .ToList();
  }

  // awards 
  public List<Award> GetAwardsByTconst(string tconst)
  {
    return _context.Awards.Where(a => a.Tconst == tconst).ToList();
  }

  // overall rating
  public List<OverallRating> GetOverallRatings(string tconst)
  {
    return _context.OverallRatings.Where(a => a.Tconst == tconst).ToList();
  }
}

