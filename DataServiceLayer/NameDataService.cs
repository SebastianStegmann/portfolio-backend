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

  // All professions for a person
  public List<Profession> GetNameProfessions(string nconst)
  {
    return _context.NameProfessions
      .Where(np => np.Nconst == nconst)
      .Join(_context.Professions,
          np => np.ProfessionId,
          p => p.Id,
          (np, p) => p)
      .ToList();
  }

    // All people with a specific profession
    public List<NameBasics> GetNamesByProfession(int professionId)
    {
        return _context.NameProfessions
          .Where(np => np.ProfessionId == professionId)
          .Join(_context.NameBasics,
              np => np.Nconst,
              nb => nb.Nconst,
              (np, nb) => nb)
          .ToList();
    }

  // All people known for a specific movie
  public List<NameBasics> GetNamesKnownForTitle(string tconst)
  {
    return _context.KnownFors
      .Where(kf => kf.Tconst == tconst)
      .Join(_context.NameBasics,
          kf => kf.Nconst,
          nb => nb.Nconst,
          (kf, nb) => nb)
      .ToList();
  }

  // All professions
  public List<Profession> GetAllProfessions()
  {
    return _context.Professions.ToList();
  }

    // All roles for a specific person in a specific title
    public List<Role> GetNameRolesInTitle(string nconst, string tconst)
    {
        return _context.NameTitleRoles
          .Where(ntr => ntr.Nconst == nconst && ntr.Tconst == tconst)
          .Join(_context.Roles,
              ntr => ntr.RoleId,
              r => r.RoleId,
              (ntr, r) => r)
          .ToList();
    }

    // All people with a specific role in a title
    public List<NameBasics> GetNamesByRoleInTitle(string tconst, int roleId)
    {
        return _context.NameTitleRoles
          .Where(ntr => ntr.Tconst == tconst && ntr.RoleId == roleId)
          .Join(_context.NameBasics,
              ntr => ntr.Nconst,
              nb => nb.Nconst,
              (ntr, nb) => nb)
          .ToList();
    }

    // All roles for a person across all titles
    public List<Role> GetAllRolesForName(string nconst)
    {
        return _context.NameTitleRoles
          .Where(ntr => ntr.Nconst == nconst)
          .Join(_context.Roles,
              ntr => ntr.RoleId,
              r => r.RoleId,
              (ntr, r) => r)
          .Distinct()
          .ToList();
    }

    // All titles where a person had a specific role
    public List<TitleBasics> GetTitlesByNameAndRole(string nconst, int roleId)
    {
        return _context.NameTitleRoles
          .Where(ntr => ntr.Nconst == nconst && ntr.RoleId == roleId)
          .Join(_context.TitleBasics,
              ntr => ntr.Tconst,
              tb => tb.Tconst,
              (ntr, tb) => tb)
          .ToList();
    }


    // Helpers
    // Check if a name has any known titles (queries database directly)
    public bool HasKnownForTitles(string nconst)
    {
        return _context.KnownFors.Any(kf => kf.Nconst == nconst);
    }
}
