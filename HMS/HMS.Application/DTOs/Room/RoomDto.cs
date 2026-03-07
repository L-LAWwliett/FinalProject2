using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Application.DTOs.Room
{
    // რასაც ვაბრუნებთ API-დან (GET)
    public class RoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int HotelId { get; set; } // რომელ სასტუმროს ეკუთვნის
    }
}
