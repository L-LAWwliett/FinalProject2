using HMS.Application.Interfaces;
using HMS.Domain.Entities;
using HMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HMS.Infrastructure.Repositories
{
    public class GuestRepository : GenericRepository<Guest>, IGuestRepository
    {
        public GuestRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> PersonalNumberExistsAsync(string personalNumber)
        {
            return await _context.Guests.AnyAsync(g => g.PersonalNumber == personalNumber);
        }
    }
}