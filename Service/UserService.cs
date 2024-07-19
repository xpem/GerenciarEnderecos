using Domain;
using Domain.Models;
using Domain.Requests.User;
using Domain.Responses.User;
using Infra;

namespace Service
{
    public class UserService(IEncryptService encryptService, IUserRepo userRepo,IJwtFunctions jwtFunctions) : IUserService
    {
        public async Task<BaseResponse> CreateAsync(UserRequest userReq)
        {
            string? validateError = userReq.Validate();

            if (!string.IsNullOrEmpty(validateError)) return new BaseResponse(null, validateError);

            User user = new() { Name = userReq.Name, Email = userReq.Email, Password = userReq.Password, CreatedAt = DateTime.Now };

            string? existingUserMessage = await ValidateExisting(user);
            if (existingUserMessage != null) { return new BaseResponse(null, existingUserMessage); }

            if (user.Password != null)
                user.Password = encryptService.Encrypt(user.Password);
            else throw new NullReferenceException("Password do usuario nulo");

            await userRepo.Create(user);

            UserResponse? userRes;

            if (user?.Id is not null)
                userRes = new() { Id = user.Id, Name = user.Name, Email = user.Email, CreatedAt = user.CreatedAt };
            else throw new NullReferenceException("Id do usuário nulo");

            return new BaseResponse(userRes);
        }

        public async Task<BaseResponse> GenerateUserTokenAsync(UserSessionRequest reqUserSession)
        {
            string? validateError = reqUserSession.Validate();

            if (!string.IsNullOrEmpty(validateError)) return new BaseResponse(null, validateError);

            User? userResp = await userRepo.GetUserByEmailAndPasswordAsync(reqUserSession.Email, encryptService.Encrypt(reqUserSession.Password));

            if (userResp is null) return new BaseResponse(null, "User/Password incorrect");

            string userJwt = jwtFunctions.GenerateToken(userResp.Id, userResp.Email, DateTime.UtcNow.AddDays(5));

            return new BaseResponse(userJwt);
        }

        protected async Task<string?> ValidateExisting(User user)
        {
            User? userResp = await userRepo.GetByNameOrEmailAsync(user.Name, user.Email);

            if (userResp != null)
            {
                if (userResp.Name.Equals(user.Name))
                    return "User Name already exists.";

                if (userResp.Email.Equals(user.Email))
                    return "User Email already exists.";
            }

            return null;
        }
    }
}
