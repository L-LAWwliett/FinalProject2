using HMS.Application.Interfaces;
using HMS.Domain.Entities;
using HMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Infrastructure.Repositories
{
    public class HotelRepository : GenericRepository<Hotel>, IHotelRepository
    {
        public HotelRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Hotel>> GetFilteredHotelsAsync(string? country, string? city, int? rating)
        {
            // ვიწყებთ ბაზის Query-ს
            var query = _context.Hotels.AsQueryable();

            // ვამატებთ ფილტრებს, თუ მომხმარებელმა მოგვაწოდა პარამეტრები
            if (!string.IsNullOrEmpty(country))
                query = query.Where(h => h.Country == country);

            if (!string.IsNullOrEmpty(city))
                query = query.Where(h => h.City == city);

            if (rating.HasValue)
                query = query.Where(h => h.Rating == rating.Value);

            return await query.ToListAsync();
        }

        public async Task<bool> HasRoomsAsync(int hotelId)
        {
            // ამოწმებს, მოიძებნება თუ არა თუნდაც ერთი ოთახი ამ სასტუმროს ID-ით
            return await _context.Rooms.AnyAsync(r => r.HotelId == hotelId);
        }
    }
}