using AutoMapper;
using HMS.Application.DTOs.Hotel;
using HMS.Application.Interfaces;
using HMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Application.Services
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;

        // Dependency Injection-ით შემოგვაქვს Repository და Mapper
        public HotelService(IHotelRepository hotelRepository, IMapper mapper)
        {
            _hotelRepository = hotelRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<HotelDto>> GetHotelsAsync(string? country, string? city, int? rating)
        {
            // მოგვაქვს გაფილტრული მონაცემები ბაზიდან
            var hotels = await _hotelRepository.GetFilteredHotelsAsync(country, city, rating);

            // ვაბრუნებთ DTO-დ გადაქცეულ სიას
            return _mapper.Map<IEnumerable<HotelDto>>(hotels);
        }

        public async Task<HotelDto?> GetHotelByIdAsync(int id)
        {
            var hotel = await _hotelRepository.GetByIdAsync(id);
            if (hotel == null) return null;

            return _mapper.Map<HotelDto>(hotel);
        }

        public async Task<HotelDto> CreateHotelAsync(CreateHotelDto createHotelDto)
        {
            // DTO-ს ვაქცევთ Hotel Entity-დ
            var hotelEntity = _mapper.Map<Hotel>(createHotelDto);

            // ვინახავთ ბაზაში
            var createdHotel = await _hotelRepository.AddAsync(hotelEntity);

            // დაბრუნებისას ისევ DTO-დ ვაქცევთ (უკვე მინიჭებული ID-ით)
            return _mapper.Map<HotelDto>(createdHotel);
        }

        public async Task UpdateHotelAsync(int id, UpdateHotelDto updateHotelDto)
        {
            var hotel = await _hotelRepository.GetByIdAsync(id);
            if (hotel == null)
                throw new Exception("სასტუმრო ვერ მოიძებნა.");

            // AutoMapper-ს გადააქვს ახალი მონაცემები არსებულ ობიექტში
            _mapper.Map(updateHotelDto, hotel);

            await _hotelRepository.UpdateAsync(hotel);
        }

        public async Task DeleteHotelAsync(int id)
        {
            var hotel = await _hotelRepository.GetByIdAsync(id);
            if (hotel == null)
                throw new Exception("სასტუმრო ვერ მოიძებნა.");

            //  ბიზნეს ლოგიკის ვალიდაცია აქვს თუ არა ოთახები?
            bool hasRooms = await _hotelRepository.HasRoomsAsync(id);
            if (hasRooms)
            {
                // თუ აქვს, ვისვრით ერორს 
                throw new InvalidOperationException("სასტუმროს წაშლა შეუძლებელია, რადგან მას აქვს ოთახები.");
            }

            // თუ ოთახები არ აქვს,  ვშლით
            await _hotelRepository.DeleteAsync(hotel);
        }
    }
}