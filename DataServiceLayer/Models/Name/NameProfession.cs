using System;
using System.Collections.Generic;
using System.Text;

namespace DataServiceLayer.Models.Name;

public class NameProfession
{
    public string Nconst { get; set; } = string.Empty; // Can never be null, starts as ""
    public int ProfessionId { get; set; } 
}
