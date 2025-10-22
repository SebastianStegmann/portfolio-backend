using System.ComponentModel.DataAnnotations;

namespace DataServiceLayer.Models.TitleBasics;

public class TitlePrincipal 
{
    [Key]
    public string? Tconst { get; set; }
    public short? Ordering { get; set; }
    public string? Nconst { get; set; }
    public string? Category { get; set; }
    public string? Job { get; set; }
    public string? Characters { get; set; }
}
