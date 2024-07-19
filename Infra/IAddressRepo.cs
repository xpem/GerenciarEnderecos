using Domain.DTOs;

namespace Infra
{
    public interface IAddressRepo
    {
        Task Create(Address address);
        Task Delete(int id, int uid);
        Task<List<Address>> GetAsync(int uid);
        Task<Address?> GetByIdAsync(int id, int uid);
        Task Update(Address address);
    }
}