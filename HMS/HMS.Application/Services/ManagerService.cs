using AutoMapper;
using HMS.Application.DTOs.Manager;
using HMS.Application.Interfaces;
using HMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Application.Services
{
    public class ManagerService : IManagerService
    {
        private readonly IManagerRepository _managerRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;

        public ManagerService(
            IManagerRepository managerRepository,
            IHotelRepository hotelRepository,
            IMapper mapper)
        {
            _managerRepository = managerRepository;
            _hotelRepository = hotelRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ManagerDto>> GetAllManagersAsync()
        {
            var managers = await _managerRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ManagerDto>>(managers);
        }

        public async Task<ManagerDto?> GetManagerByIdAsync(int id)
        {
            var manager = await _managerRepository.GetByIdAsync(id);
            return manager == null ? null : _mapper.Map<ManagerDto>(manager);
        }

        public async Task<ManagerDto> CreateManagerAsync(CreateManagerDto createDto)
        {
            // 1. ვამოწმებთ, არსებობს თუ არა სასტუმრო
            var hotel = await _hotelRepository.GetByIdAsync(createDto.HotelId);
            if (hotel == null) throw new Exception("სასტუმრო ვერ მოიძებნა.");

            // 2. ვამოწმებთ ელფოსტის უნიკალურობას
            if (await _managerRepository.EmailExistsAsync(createDto.Email))
                throw new InvalidOperationException("მენეჯერი ამ ელფოსტით უკვე არსებობს.");

            // 3. ვამოწმებთ პირადი ნომრის უნიკალურობას
            if (await _managerRepository.PersonalNumberExistsAsync(createDto.PersonalNumber))
                throw new InvalidOperationException("მენეჯერი ამ პირადი ნომრით უკვე არსებობს.");

            var managerEntity = _mapper.Map<Manager>(createDto);
            var createdManager = await _managerRepository.AddAsync(managerEntity);

            return _mapper.Map<ManagerDto>(createdManager);
        }

        public async Task UpdateManagerAsync(int id, UpdateManagerDto updateDto)
        {
            var manager = await _managerRepository.GetByIdAsync(id);
            if (manager == null) throw new Exception("მენეჯერი ვერ მოიძებნა.");

            _mapper.Map(updateDto, manager);
            await _managerRepository.UpdateAsync(manager);
        }

        public async Task DeleteManagerAsync(int id)
        {
            var manager = await _managerRepository.GetByIdAsync(id);
            if (manager == null) throw new Exception("მენეჯერი ვერ მოიძებნა.");

            // მთავარი ბიზნეს ლოგიკა: ვამოწმებთ, ჰყავს თუ არა სასტუმროს სხვა მენეჯერიც
            int managerCount = await _managerRepository.GetManagersCountByHotelIdAsync(manager.HotelId);
            if (managerCount <= 1)
                throw new InvalidOperationException("შეუძლებელია მენეჯერის წაშლა, რადგან ის ერთადერთია ამ სასტუმროში.");

            await _managerRepository.DeleteAsync(manager);
        }
    }
}