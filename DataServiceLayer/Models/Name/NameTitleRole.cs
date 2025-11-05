using System;
using System.Collections.Generic;
using System.Text;

namespace DataServiceLayer.Models.Name;

public class NameTitleRole
{
    public required string Nconst { get; set; }
    public required string Tconst { get; set; }
    public int RoleId { get; set; }
}
