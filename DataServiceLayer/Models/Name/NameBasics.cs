using System;
using System.Collections.Generic;
using System.Text;
using DataServiceLayer.Models.Title;
using TitleBasicsModel = DataServiceLayer.Models.Title.TitleBasics;

namespace DataServiceLayer.Models.Name;

public class NameBasics
{
    public required string Nconst { get; set; }
    public string? Name { get; set; }
    public int? BirthYear { get; set; }
    public int? DeathYear { get; set; }
    public decimal? NameRating { get; set; }

    public ICollection<TitleBasicsModel> Titles { get; set; } = new List<TitleBasicsModel>();
}
