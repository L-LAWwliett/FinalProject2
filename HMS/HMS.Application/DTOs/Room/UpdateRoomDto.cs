using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Application.DTOs.Room
{
    // რასაც ვითხოვთ განახლებისას (PUT)
    public class UpdateRoomDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
