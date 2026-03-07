using AutoMapper;
using HMS.Application.DTOs.Room;
using HMS.Application.Interfaces;
using HMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Application.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IHotelRepository _hotelRepository; // გვჭირდება სასტუმროს შესამოწმებლად
        private readonly IMapper _mapper;

        public RoomService(IRoomRepository roomRepository, IHotelRepository hotelRepository, IMapper mapper)
        {
            _roomRepository = roomRepository;
            _hotelRepository = hotelRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoomDto>> GetRoomsByHotelIdAsync(int hotelId)
        {
            var rooms = await _roomRepository.GetRoomsByHotelIdAsync(hotelId);
            return _mapper.Map<IEnumerable<RoomDto>>(rooms);
        }

        public async Task<RoomDto?> GetRoomByIdAsync(int id)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            return room == null ? null : _mapper.Map<RoomDto>(room);
        }

        public async Task<RoomDto> CreateRoomAsync(CreateRoomDto createRoomDto)
        {
            // 1. ვამოწმებთ, არსებობს თუ არა ასეთი სასტუმრო ბაზაში
            var hotelExists = await _hotelRepository.GetByIdAsync(createRoomDto.HotelId);
            if (hotelExists == null)
                throw new Exception("სასტუმრო მითითებული ID-ით ვერ მოიძებნა.");

            // 2. ვამოწმებთ ფასს (თუმცა ბაზის დონეზეც გვიცავს CheckConstraint)
            if (createRoomDto.Price <= 0)
                throw new ArgumentException("ოთახის ფასი უნდა იყოს 0-ზე მეტი.");

            var roomEntity = _mapper.Map<Room>(createRoomDto);
            var createdRoom = await _roomRepository.AddAsync(roomEntity);

            return _mapper.Map<RoomDto>(createdRoom);
        }

        public async Task UpdateRoomAsync(int id, UpdateRoomDto updateRoomDto)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            if (room == null)
                throw new Exception("ოთახი ვერ მოიძებნა.");

            if (updateRoomDto.Price <= 0)
                throw new ArgumentException("ოთახის ფასი უნდა იყოს 0-ზე მეტი.");

            _mapper.Map(updateRoomDto, room);
            await _roomRepository.UpdateAsync(room);
        }

        public async Task DeleteRoomAsync(int id)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            if (room == null)
                throw new Exception("ოთახი ვერ მოიძებნა.");

            await _roomRepository.DeleteAsync(room);
        }
    }
}