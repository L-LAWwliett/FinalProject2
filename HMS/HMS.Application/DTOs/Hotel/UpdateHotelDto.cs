using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Application.DTOs.Hotel
{
    // 3. რასაც ვიღებთ განახლებისას (PUT)
    public class UpdateHotelDto
    {
        public string Name { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
