using System;
using System.Collections.Generic;
using System.Text;
using DataServiceLayer.Models.TitleBasics;
using TitleBasicsModel = DataServiceLayer.Models.TitleBasics.TitleBasics;

namespace DataServiceLayer.Models.NameBasics;

public class NameBasics
{
    public string Nconst { get; set; } = string.Empty; // Can never be null, starts as ""
    public string? Name { get; set; }
    public int? BirthYear { get; set; }
    public int? DeathYear { get; set; }
    public decimal? NameRating { get; set; }

    public ICollection<TitleBasicsModel> Titles { get; set; } = new List<TitleBasicsModel>();
}
