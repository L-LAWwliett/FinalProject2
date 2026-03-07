using HMS.Application.DTOs.Guest;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Application.Interfaces
{
    public interface IGuestService
    {
        Task<IEnumerable<GuestDto>> GetAllGuestsAsync();
        Task<GuestDto?> GetGuestByIdAsync(int id);
        Task<GuestDto> CreateGuestAsync(CreateGuestDto createGuestDto);
        Task UpdateGuestAsync(int id, UpdateGuestDto updateGuestDto);
        Task DeleteGuestAsync(int id);
    }
}