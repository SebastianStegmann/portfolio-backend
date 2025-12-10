using DataServiceLayer.Models;
using DataServiceLayer.Models.Name;
using DataServiceLayer.Models.Title;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration.Assemblies;
using System.Text;

namespace DataServiceLayer;

public class NameDataService : BaseDataService
{
  public NameDataService(ImdbContext context) : base(context) {
    Console.WriteLine("Context injected");
    if (_context == null) Console.WriteLine("Context is null!");}

  //Name

  public int GetNamesCount()
  {
    return _context.NameBasics.Count();
  }

  public List<NameBasics> GetNames(int page, int pageSize)
  {
    return _context.NameBasics
      .Include(x => x.Titles)
      .OrderBy(x => x.Nconst)
      .Skip(page * pageSize)
      .Take(pageSize)
      .ToList();
  }

  public NameBasics? GetName(string nconst)
  {
    return _context.NameBasics
      .Include(n => n.Titles)
      .FirstOrDefault(n => n.Nconst == nconst);

  }

  // THe movies that the Actor is known for
  public List<TitleBasics> GetKnownForTitles(string nconst)
  {
    return _context.KnownFors
      .Where(kf => kf.Nconst == nconst)
      .Join(_context.TitleBasics,
          kf => kf.Tconst,
          tb => tb.Tconst,
          (kf, tb) => tb)
      .ToList();
  }

  public List<object> GetNameProfessions(string nconst)
  {
    return _context.NameProfessions
      .Where(np => np.Nconst == nconst)
      .Join(_context.Professions,
          np => np.ProfessionId,
          p => p.Id,
          (np, p) => new 
          {
          Id = p.Id,
          Name = p.ProfessionName
          })
    .ToList<object>();
  }

  public List<TitleBasics> GetTitlesByNconst(string nconst)
  {
    return _context.TitlePrincipals
      .Where(tp => tp.Nconst == nconst)
      .Join(_context.TitleBasics,
          tp => tp.Tconst,
          tb => tb.Tconst,
          (tp, tb) => tb)
      .Distinct()
      .OrderBy(tb => tb.ReleaseDate)
      .ToList();
  }


  // All professions
  public List<Profession> GetAllProfessions()
  {
    return _context.Professions.ToList();
  }


  // Helpers
  // Check if a name has any known titles (queries database directly)
  public bool HasKnownForTitles(string nconst)
  {
    return _context.KnownFors.Any(kf => kf.Nconst == nconst);
  }
}
