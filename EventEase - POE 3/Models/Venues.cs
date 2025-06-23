﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace EventEase.Models
{



    using EventEase.Models;

    public class Venues
    {
        [Key]
        public int VenueID { get; set; }
        public string? VenueName { get; set; }
        public string? Location { get; set; }
        public int Capacity { get; set; }
        public string? Description { get; set; }
        public string? ImageURL { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public bool IsAvailable { get; set; } = true;

        public List<Events> Events { get; set; } = new();
    }
}
