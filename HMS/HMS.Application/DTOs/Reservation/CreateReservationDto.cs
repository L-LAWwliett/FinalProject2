using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Application.DTOs.Reservation
{
    // რასაც ვითხოვთ კლიენტისგან ჯავშნის შექმნისას
    public class CreateReservationDto
    {
        public int GuestId { get; set; }
        public List<int> RoomIds { get; set; } = new List<int>(); // <-- კლიენტი გვაწვდის ოთახების სიას
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

    }
}
