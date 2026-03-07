using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Application.Interfaces
{
    // <T> ნიშნავს, რომ ეს ინტერფეისი მიიღებს ნებისმიერ კლასს (Hotel, Room...)
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}