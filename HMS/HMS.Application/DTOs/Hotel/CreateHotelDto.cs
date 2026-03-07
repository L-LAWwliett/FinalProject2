using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Application.DTOs.Hotel
{
    // 2. რასაც ვიღებთ შექმნისას (POST)
    public class CreateHotelDto
    {
        public string Name { get; set; } = string.Empty;
        public int Rating { get; set; } // 1-5
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
