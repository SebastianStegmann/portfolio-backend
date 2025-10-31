using System;
using System.Collections.Generic;
using System.Text;

namespace DataServiceLayer.Models.DTO
{
    public class CastMember
    {
        public string Nconst { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? Category { get; set; }
        public string? Job { get; set; }
        public string? Characters { get; set; }
        public short? Ordering { get; set; }
    }
}
