using Domain;
using Domain.Requests.User;

namespace Service
{
    public interface IUserService
    {
        Task<BaseResponse> CreateAsync(UserRequest userReq);
        Task<BaseResponse> GenerateUserTokenAsync(UserSessionRequest reqUserSession);
    }
}