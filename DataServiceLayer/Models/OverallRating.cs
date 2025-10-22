using System;
using System.Collections.Generic;
using System.Text;

namespace DataServiceLayer.Models;

public class OverallRating
{
    public string Tconst { get; set; } = string.Empty;
    public int Rating { get; set; }
    public int Votes { get; set; }
}
