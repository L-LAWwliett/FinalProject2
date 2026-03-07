using HMS.Domain.Entities;
using System.Threading.Tasks;

namespace HMS.Application.Interfaces
{
    public interface IManagerRepository : IGenericRepository<Manager>
    {
        Task<bool> EmailExistsAsync(string email);
        Task<bool> PersonalNumberExistsAsync(string personalNumber);

        // ვითვლით კონკრეტულ სასტუმროში რამდენი მენეჯერია
        Task<int> GetManagersCountByHotelIdAsync(int hotelId);
    }
}