using Domain.Models;

namespace Infra
{
    public interface IUserRepo
    {
        Task<int> Create(User user);
        Task<User?> GetByNameOrEmailAsync(string name, string email);
        Task<User?> GetUserByEmailAndPasswordAsync(string email, string encryptedPassword);
    }
}