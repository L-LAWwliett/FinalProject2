using HMS.Application.DTOs.Reservation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Application.Interfaces
{
    public interface IReservationService
    {
        Task<IEnumerable<ReservationDto>> GetAllReservationsAsync();
        Task<ReservationDto?> GetReservationByIdAsync(int id);
        Task<ReservationDto> CreateReservationAsync(CreateReservationDto createDto);
        Task UpdateReservationAsync(int id, UpdateReservationDto updateDto);
        Task DeleteReservationAsync(int id);
    }
}