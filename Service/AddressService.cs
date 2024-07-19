using Domain;
using Domain.DTOs;
using Domain.Requests;
using Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AddressService(IAddressRepo addressRepo)
    {
        public  async Task<BaseResponse> Create(AddressRequest addressRequest, int uid)
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
                Number = addressRequest.Number,
                State = addressRequest.State,
                Street = addressRequest.Street,
                UserId = uid
            };

            await addressRepo.Create(address);

            return new BaseResponse(address);
        }
    }
}
