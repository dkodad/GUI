﻿using System.ComponentModel.DataAnnotations;

namespace Razor.Models.Entities
{
    public class Article
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        //Added Author
        public string Author { get; set; }
    }
}
