using HMS.Application.Interfaces;
using HMS.Domain.Entities;
using HMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Infrastructure.Repositories
{
    public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository
    {
        public ReservationRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> AreRoomsAvailableAsync(List<int> roomIds, DateTime checkIn, DateTime checkOut)
        {
            // ვამოწმებთ, შუალედურ ცხრილში ხომ არ ფიქსირდება რომელიმე ოთახი ამ თარიღებში
            bool isBooked = await _context.ReservationRooms
                .Include(rr => rr.Reservation)
                .AnyAsync(rr => roomIds.Contains(rr.RoomId) &&
                                (checkIn < rr.Reservation.CheckOutDate && checkOut > rr.Reservation.CheckInDate));

            return !isBooked;
        }
    }
}