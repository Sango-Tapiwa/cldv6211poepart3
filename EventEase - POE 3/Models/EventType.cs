using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EventEase.Models
{
    public class EventType
    {
        public int EventTypeID { get; set; }
        public string Name { get; set; }

        // Navigation Property
        public ICollection<Events> Events { get; set; }
    }
}
