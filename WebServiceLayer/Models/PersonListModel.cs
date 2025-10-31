﻿namespace WebServiceLayer.Models
{
    public class PersonListModel
    {
        public string? URL { get; set; }
        public string Name { get; set; } = null!;
        public DateTime? LastLogin { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
