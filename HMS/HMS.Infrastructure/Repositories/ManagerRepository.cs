using HMS.Application.Interfaces;
using HMS.Domain.Entities;
using HMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HMS.Infrastructure.Repositories
{
    public class ManagerRepository : GenericRepository<Manager>, IManagerRepository
    {
        public ManagerRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Managers.AnyAsync(m => m.Email == email);
        }

        public async Task<bool> PersonalNumberExistsAsync(string personalNumber)
        {
            return await _context.Managers.AnyAsync(m => m.PersonalNumber == personalNumber);
        }

        public async Task<int> GetManagersCountByHotelIdAsync(int hotelId)
        {
            return await _context.Managers.CountAsync(m => m.HotelId == hotelId);
        }
    }
}