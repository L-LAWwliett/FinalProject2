using HMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Application.Interfaces
{
    public interface IReservationRepository : IGenericRepository<Reservation>
    {
        
        Task<bool> AreRoomsAvailableAsync(List<int> roomIds, DateTime checkIn, DateTime checkOut);
    }
}