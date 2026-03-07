using AutoMapper;
using HMS.Application.DTOs.Guest;
using HMS.Application.Interfaces;
using HMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Application.Services
{
    public class GuestService : IGuestService
    {
        private readonly IGuestRepository _guestRepository;
        private readonly IMapper _mapper;

        public GuestService(IGuestRepository guestRepository, IMapper mapper)
        {
            _guestRepository = guestRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GuestDto>> GetAllGuestsAsync()
        {
            var guests = await _guestRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<GuestDto>>(guests);
        }

        public async Task<GuestDto?> GetGuestByIdAsync(int id)
        {
            var guest = await _guestRepository.GetByIdAsync(id);
            return guest == null ? null : _mapper.Map<GuestDto>(guest);
        }

        public async Task<GuestDto> CreateGuestAsync(CreateGuestDto createGuestDto)
        {
            //  ვალიდაცია: ვამოწმებთ, ხომ არ არსებობს უკვე სტუმარი ამ პირადი ნომრით
            if (await _guestRepository.PersonalNumberExistsAsync(createGuestDto.PersonalNumber))
            {
                //  Middleware ამას დაიჭერს და 400 Bad Request-ს დააბრუნებს
                throw new InvalidOperationException("სტუმარი ამ პირადი ნომრით უკვე არსებობს სისტემაში.");
            }

            var guestEntity = _mapper.Map<Guest>(createGuestDto);
            var createdGuest = await _guestRepository.AddAsync(guestEntity);

            return _mapper.Map<GuestDto>(createdGuest);
        }

        public async Task UpdateGuestAsync(int id, UpdateGuestDto updateGuestDto)
        {
            var guest = await _guestRepository.GetByIdAsync(id);
            if (guest == null)
                throw new Exception("სტუმარი ვერ მოიძებნა.");

            _mapper.Map(updateGuestDto, guest);
            await _guestRepository.UpdateAsync(guest);
        }

        public async Task DeleteGuestAsync(int id)
        {
            var guest = await _guestRepository.GetByIdAsync(id);
            if (guest == null)
                throw new Exception("სტუმარი ვერ მოიძებნა.");

            await _guestRepository.DeleteAsync(guest);
        }
    }
}