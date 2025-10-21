using DataServiceLayer.Models;
using DataServiceLayer.Models.NameBasics;
namespace DataServiceLayer;

public class DataService
{
  private readonly ImdbContext _context;

  public DataService(ImdbContext context)
  {
    _context = context;
  }

    //Title

  public List<TitleBasics> GetTitles()
  {
    return _context.TitleBasics.ToList();
  }

  public TitleBasics? GetTitle(string tconst)
  {
    return _context.TitleBasics.Find(tconst);
  }

    //Name

    public List<NameBasics> GetNames()
    {
        return _context.NameBasics.ToList();
    }

    public NameBasics? GetName(string nconst)
    {
        return _context.NameBasics.Find(nconst);
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
    public List<NameBasics> GetNamesByProfession(int professionId)  // Changed parameter type to int
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
}
