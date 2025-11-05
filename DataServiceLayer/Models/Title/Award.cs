using System;
using System.Collections.Generic;
using System.Text;

namespace DataServiceLayer.Models.Title;

public class Award
{
    public required string Tconst { get; set; }
    public string? AwardInfo { get; set; }
}
