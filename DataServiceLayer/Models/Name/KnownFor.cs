using System;
using System.Collections.Generic;
using System.Text;

namespace DataServiceLayer.Models.NameBasics;

public class KnownFor
{
    public string Nconst { get; set; } = string.Empty; // Can never be null, starts as ""
    public string Tconst { get; set; } = string.Empty; // Can never be null, starts as ""
}
