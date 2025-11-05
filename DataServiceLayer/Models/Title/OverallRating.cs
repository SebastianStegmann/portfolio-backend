using System;
using System.Collections.Generic;
using System.Text;

namespace DataServiceLayer.Models.Title;

public class OverallRating
{
    public required string Tconst { get; set; }
    public int Rating { get; set; }
    public int Votes { get; set; }
}
