using System;
using System.Collections.Generic;
using System.Text;

namespace DataServiceLayer.Models.NameBasics;

public class NameBasics
{
    public string Nconst { get; set; } = string.Empty; // Can never be null, starts as ""
    public string? Name { get; set; }
    public string? BirthYear { get; set; }
    public string? DeathYear { get; set; }
    public string? NameRating { get; set; }
}
