using Microsoft.EntityFrameworkCore;
using OfficePass.Domain.Entities;
using OfficePass.Domain.Repositories;
using OfficePass.Domain.Repositories.Abstract;
using OfficePass.Enums;
using OfficePass.Helpers;
using OfficePass.Models;
using System.Data;
using System.Security.Claims;

namespace OfficePass.Services
{
    public class LoginService : ILoginService
    {
        private readonly LoginRepository loginRepository;

        public LoginService(LoginRepository loginRepository)
        {
            if (loginRepository == null) throw new ArgumentNullException(nameof(loginRepository));
            this.loginRepository = loginRepository;
        }

        public Task<IBaseResponse<User>> GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IBaseResponse<List<User>>> GetUsers()
        {
            throw new NotImplementedException();
        }

        public async Task<IBaseResponse<ClaimsIdentity>> Login(LoginViewModel model)
        {
            try
            {
                var user = await loginRepository.GetAll()
                    .Include(r => r.Role)
                    .FirstOrDefaultAsync(u => u.Login == model.UserName);
                if (user == null)
                {
                    return new BaseResponse<ClaimsIdentity>
                    {
                        Description = "Пользователь не найден",
                        StatusCode = StatusCode.NotFound
                    };
                }

                if (user.Password != HashPasswordHelper.HashPassowrd(model.Password))
                {
                    return new BaseResponse<ClaimsIdentity>
                    {
                        Description = "Не верный пароль",
                        StatusCode = StatusCode.IncorrectPassword
                    };
                }

                var result = Authenticate(user);
                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = result,
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        private ClaimsIdentity Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name),
                new Claim("LoginId", user.Id.ToString())
            };
            return new ClaimsIdentity(claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }
    }
}
