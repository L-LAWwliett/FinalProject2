using HMS.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Application.Interfaces
{
    public interface IHotelRepository : IGenericRepository<Hotel>
    {
        // ფილტრაციის მეთოდი
        Task<IEnumerable<Hotel>> GetFilteredHotelsAsync(string? country, string? city, int? rating);

        // შემოწმება: აქვს თუ არა სასტუმროს ოთახები (წაშლის ვალიდაციისთვის)
        Task<bool> HasRoomsAsync(int hotelId);
    }
}