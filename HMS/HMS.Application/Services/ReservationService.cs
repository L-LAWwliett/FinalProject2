using AutoMapper;
using HMS.Application.DTOs.Reservation;
using HMS.Application.Interfaces;
using HMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IGuestRepository _guestRepository;
        private readonly IMapper _mapper;

        public ReservationService(
            IReservationRepository reservationRepository,
            IRoomRepository roomRepository,
            IGuestRepository guestRepository,
            IMapper mapper)
        {
            _reservationRepository = reservationRepository;
            _roomRepository = roomRepository;
            _guestRepository = guestRepository;
            _mapper = mapper;
        }

        

        public async Task<IEnumerable<ReservationDto>> GetAllReservationsAsync()
        {
            var reservations = await _reservationRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ReservationDto>>(reservations);
        }

        public async Task<ReservationDto?> GetReservationByIdAsync(int id)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);
            return reservation == null ? null : _mapper.Map<ReservationDto>(reservation);
        }

        public async Task<ReservationDto> CreateReservationAsync(CreateReservationDto createDto)
        {
            // 1. ვალიდაცია თარიღებზე
            if (createDto.CheckInDate.Date < DateTime.UtcNow.Date)
                throw new ArgumentException("Check-in თარიღი არ შეიძლება იყოს წარსულში.");
            if (createDto.CheckOutDate <= createDto.CheckInDate)
                throw new ArgumentException("Check-out თარიღი უნდა იყოს Check-in-ზე გვიან.");

            // 2. სტუმრის შემოწმება
            var guest = await _guestRepository.GetByIdAsync(createDto.GuestId);
            if (guest == null) throw new Exception("სტუმარი ვერ მოიძებნა.");

            // 3. ოთახების არსებობის შემოწმება და ჯამური ფასის დათვლა
            decimal totalRoomsPrice = 0;
            var reservationRooms = new List<ReservationRoom>();

            foreach (var roomId in createDto.RoomIds)
            {
                var room = await _roomRepository.GetByIdAsync(roomId);
                if (room == null) throw new Exception($"ოთახი ID-ით {roomId} ვერ მოიძებნა.");

                totalRoomsPrice += room.Price;
                reservationRooms.Add(new ReservationRoom { RoomId = roomId }); // ვამზადებთ შუალედური ცხრილისთვის
            }

            // 4. ხელმისაწვდომობის შემოწმება ერთიანად (ყველა ოთახისთვის)
            bool areAvailable = await _reservationRepository.AreRoomsAvailableAsync(createDto.RoomIds, createDto.CheckInDate, createDto.CheckOutDate);
            if (!areAvailable)
                throw new InvalidOperationException("არჩეული ოთახებიდან ერთ-ერთი ან ყველა უკვე დაჯავშნილია ამ თარიღებში.");

            // 5. ჯავშნის შექმნა 
            var reservationEntity = new Reservation
            {
                GuestId = createDto.GuestId,
                CheckInDate = createDto.CheckInDate,
                CheckOutDate = createDto.CheckOutDate,
                ReservationRooms = reservationRooms // ვურთავთ ოთახების სიას
            };

            var createdReservation = await _reservationRepository.AddAsync(reservationEntity);

            // 6. DTO-ს დაბრუნება 
            int totalDays = (createDto.CheckOutDate - createDto.CheckInDate).Days;
            if (totalDays == 0) totalDays = 1;

            return new ReservationDto
            {
                Id = createdReservation.Id,
                GuestId = createdReservation.GuestId,
                RoomIds = createDto.RoomIds,
                CheckInDate = createdReservation.CheckInDate,
                CheckOutDate = createdReservation.CheckOutDate,
                TotalPrice = totalDays * totalRoomsPrice
            };
        }

        public async Task UpdateReservationAsync(int id, UpdateReservationDto updateDto)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);
            if (reservation == null) throw new Exception("ჯავშანი ვერ მოიძებნა.");

            if (updateDto.CheckOutDate <= updateDto.CheckInDate)
                throw new ArgumentException("Check-out თარიღი უნდა იყოს Check-in-ზე გვიან.");

            _mapper.Map(updateDto, reservation);
            await _reservationRepository.UpdateAsync(reservation);
        }

        public async Task DeleteReservationAsync(int id)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);
            if (reservation == null) throw new Exception("ჯავშანი ვერ მოიძებნა.");

            await _reservationRepository.DeleteAsync(reservation);
        }
    }
}