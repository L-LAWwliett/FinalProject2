using HMS.Application.DTOs.Manager;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Application.Interfaces
{
    public interface IManagerService
    {
        Task<IEnumerable<ManagerDto>> GetAllManagersAsync();
        Task<ManagerDto?> GetManagerByIdAsync(int id);
        Task<ManagerDto> CreateManagerAsync(CreateManagerDto createDto);
        Task UpdateManagerAsync(int id, UpdateManagerDto updateDto);
        Task DeleteManagerAsync(int id);
    }
}