using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Application.DTOs.Guest
{
    public class CreateGuestDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PersonalNumber { get; set; } = string.Empty; // ვალიდაციისთვის მნიშვნელოვანია (უნიკალური უნდა იყოს)
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
