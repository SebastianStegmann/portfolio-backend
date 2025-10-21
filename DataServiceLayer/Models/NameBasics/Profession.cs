using System;
using System.Collections.Generic;
using System.Text;

namespace DataServiceLayer.Models.NameBasics;

public class Profession
{
    public string Id { get; set; } = string.Empty; // Can never be null, starts as ""
    public string? ProfessionName { get; set; }
}
