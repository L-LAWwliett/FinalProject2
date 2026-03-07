using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Application.DTOs.Manager
{
    public class UpdateManagerDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        // Email და PersonalNumber წესით არ უნდა იცვლებოდეს ასე მარტივად, ამიტომ აქ არ ვითხოვთ.
    }
}
