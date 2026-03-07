using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Application.DTOs.Room
{
    // რასაც ვითხოვთ ოთახის შექმნისას (POST)
    public class CreateRoomDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int HotelId { get; set; } // აუცილებლად უნდა ვიცოდეთ, რომელ სასტუმროში ვამატებთ
    }
}
