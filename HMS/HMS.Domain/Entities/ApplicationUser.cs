using Microsoft.AspNetCore.Identity;

namespace HMS.Domain.Entities
{
    // IdentityUser თავისთავად შეიცავს ველებს: Id, Email, PasswordHash, PhoneNumber და ა.შ.
    // ამიტომ აქ ვამატებთ მხოლოდ იმას, რაც სტანდარტულად არ აქვს.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}