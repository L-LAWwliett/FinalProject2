using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Application.DTOs.Auth
{
    // რასაც ვითხოვთ რეგისტრაციისას
    public class RegisterDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        // სტანდარტულად ყველა ვინც რეგისტრირდება არის "Guest", მაგრამ შეგვიძლია გადავაწოდოთ "Manager" ან "Admin"
        public string Role { get; set; } = "Guest";
    }
}
