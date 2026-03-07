using HMS.Domain.Entities;
using System.Threading.Tasks;

namespace HMS.Application.Interfaces
{
    public interface IGuestRepository : IGenericRepository<Guest>
    {
        // ამოწმებს, არსებობს თუ არა ბაზაში კონკრეტული პირადი ნომერი
        Task<bool> PersonalNumberExistsAsync(string personalNumber);
    }
}