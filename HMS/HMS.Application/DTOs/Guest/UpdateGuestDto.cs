using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Application.DTOs.Guest
{
    public class UpdateGuestDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty; 
        
        
        // PersonalNumber-ს განახლებისას არ ვითხოვთ, რადგან პირადი ნომერი წესით არ იცვლება

    }
}
