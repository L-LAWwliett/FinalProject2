using HMS.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Application.Interfaces
{
    public interface IRoomRepository : IGenericRepository<Room>
    {
        // დამატებითი მეთოდი კონკრეტული სასტუმროს ოთახების წამოსაღებად
        Task<IEnumerable<Room>> GetRoomsByHotelIdAsync(int hotelId);
    }
}