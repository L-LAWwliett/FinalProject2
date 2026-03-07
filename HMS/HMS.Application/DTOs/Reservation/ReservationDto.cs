using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Application.DTOs.Reservation
{
    // რასაც ვაბრუნებთ კლიენტთან
    public class ReservationDto
    {
        public int Id { get; set; }
        public int GuestId { get; set; }
        public List<int> RoomIds { get; set; } = new List<int>(); // <-- ოთახების სია
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal TotalPrice { get; set; } // დათვლილ ფასს კლიენტს  ვუბრუნებთ
    }
}
