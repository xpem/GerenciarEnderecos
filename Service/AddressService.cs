﻿using Domain;
using Domain.DTOs;
using Domain.Requests;
using Infra;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

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

        public async Task<BaseResponse> DeleteAddress(int id, int uid)
        {
            var address = await addressRepo.GetByIdAsync(id, uid);

            if (address == null)
                return new BaseResponse(null, "Invalid id");

            try
            {
                await addressRepo.DeleteAsync(id, uid);

                return new BaseResponse("");
            }
            catch (DbUpdateException /* ex */)
            {
                return new BaseResponse(null, "Não foi possivel excluir.");
            }

        }

        public async Task<BaseResponse> UpdateAsync(AddressRequest addressRequest, int uid)
        {
            try
            {
                string? validateError = addressRequest.Validate();
                if (!string.IsNullOrEmpty(validateError)) return new BaseResponse(null, validateError);

                Address? address = await addressRepo.GetByIdAsync(Convert.ToInt32(addressRequest.Id), uid);

                if (address == null)
                    return new BaseResponse(null, "Invalid id");

                address.CEP = addressRequest.CEP;
                address.City = addressRequest.City;
                address.Complement = addressRequest.Complement;
                address.Neighborhood = addressRequest.Neighborhood;
                address.Number = Convert.ToInt32(addressRequest.Number);
                address.State = addressRequest.State;
                address.Street = addressRequest.Street;

                await addressRepo.Update(address);

                return new BaseResponse("");
            }
            catch { throw; }
        }

    }
}
