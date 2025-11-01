using DataServiceLayer.Models.DTO;
using DataServiceLayer.Models.Name;
using DataServiceLayer.Models.Title;
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
        .Include(t => t.Names)
        .OrderBy(x => x.Tconst)
        .Skip(page * pageSize)
        .Take(pageSize)
        .ToList();
  }

  public TitleBasics? GetTitle(string tconst)
  {
    return _context.TitleBasics
            .AsSplitQuery()
            .Include(t => t.Names)
            .Include(t => t.Genre)
            .ThenInclude(tg => tg.Genre)
            .Include(t => t.Aka)
            .Include(t => t.Episodes)
            .Include(t => t.OverallRating)
            .Include(t => t.Award)
            .FirstOrDefault(t => t.Tconst == tconst);
    }

  public List<Genre> GetAllGenres()
  {
    return _context.Genres.ToList();
  }

  // Get all actors (names) known for a specific movie
  public List<CastMember> GetCastForTitle(string tconst)
  {
    return _context.TitlePrincipals
      .Where(tp => tp.Tconst == tconst)
      .Join(_context.NameBasics,
          tp => tp.Nconst,
          nb => nb.Nconst,
          (tp, nb) => new CastMember
          {
            Nconst = nb.Nconst,
            Name = nb.Name,
            Category = tp.Category,
            Characters = CharacterName(tp.Characters),
            Job = tp.Job,
            Ordering = tp.Ordering
          })
      .OrderBy(cm => cm.Ordering)
      .ToList();
  }

    // Get award for a specific title
    public Award? GetAwardsByTitle(string tconst)
    {
        return _context.Awards.FirstOrDefault(a => a.Tconst == tconst);
    }

    // Get overall rating for a specific title
    public OverallRating? GetOverallRatings(string tconst)
    {
        return _context.OverallRatings.FirstOrDefault(a => a.Tconst == tconst);
    }


    // Helpers
    // Check if a name has any known titles (queries database directly)
    public bool HasKnownForTitles(string nconst)
    {
        return _context.KnownFors.Any(kf => kf.Nconst == nconst);
    }

    // Cleaning the Charactername
    private static string? CharacterName(string? characters)
    {
        if (string.IsNullOrEmpty(characters))
            return null;

        // Remove brackets and quotes
        return characters
            .Replace("[", "")
            .Replace("]", "")
            .Replace("'", "")
            .Replace("\"", "");
    }
}

