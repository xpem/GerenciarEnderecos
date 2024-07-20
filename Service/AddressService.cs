using Domain;
using Domain.DTOs;
using Domain.Requests;
using Infra;

namespace Service
{
    public class AddressService(IAddressRepo addressRepo) : IAddressService
    {
        public async Task<BaseResponse> CreateAsync(AddressRequest addressRequest, int uid)
        {
            string? validateError = addressRequest.Validate();

            if (!string.IsNullOrEmpty(validateError)) return new BaseResponse(null, validateError);

            Address address = new()
            {
                CEP = addressRequest.CEP,
                City = addressRequest.City,
                Complement = addressRequest.Complement,
                CreatedAt = DateTime.Now,
                Neighborhood = addressRequest.Neighborhood,
                Number = Convert.ToInt32(addressRequest.Number),
                State = addressRequest.State,
                Street = addressRequest.Street,
                UserId = uid
            };

            await addressRepo.Create(address);

            return new BaseResponse(address);
        }

        public async Task<List<Address>> GetAsync(int uid) => await addressRepo.GetAsync(uid);

        public async Task<Address?> GetByIdAsync(int id, int uid) => await addressRepo.GetByIdAsync(id, uid);
    }
}
