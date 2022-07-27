using WarehouseMgmt.Shared.DTOs;

namespace WarehouseMgmt.Client.Services
{
    public interface IUnauthorizedUserService
    {
        Task RegisterNewUser(RegisterUserDto registerUserDto);
        Task Login(LoginDto loginDto);
        Task ResetPassword(ResetPasswordDto resetPasswordDto);
    }
}
