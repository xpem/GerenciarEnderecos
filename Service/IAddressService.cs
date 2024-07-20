using Domain;
using Domain.DTOs;
using Domain.Requests;

namespace Service
{
    public interface IAddressService
    {
        Task<BaseResponse> CreateAsync(AddressRequest addressRequest, int uid);

        Task<List<Address>> GetAsync(int uid);

        Task<Address?> GetByIdAsync(int id, int uid);

    }
}