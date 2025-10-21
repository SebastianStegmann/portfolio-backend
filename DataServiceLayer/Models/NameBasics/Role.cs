using System;
using System.Collections.Generic;
using System.Text;

namespace DataServiceLayer.Models.NameBasics;

public class Role
{
    public string RoleId { get; set; } = string.Empty; // Can never be null, starts as ""
    public string RoleName { get; set; }
}
