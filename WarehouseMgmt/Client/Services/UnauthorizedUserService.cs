using System.Net.Http.Json;
using WarehouseMgmt.Shared.DTOs;

namespace WarehouseMgmt.Client.Services
{
    public class UnauthorizedUserService : IUnauthorizedUserService
    {
        private readonly HttpClient httpClient;

        public UnauthorizedUserService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task RegisterNewUser(RegisterUserDto registerUserDto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("api/user/register", registerUserDto);
                var content = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    throw new ApplicationException(content);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Login(LoginDto loginDto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("api/user/login", loginDto);
                var content = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    throw new ApplicationException(content);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("api/user/resetPassword", resetPasswordDto);
                var content = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    throw new ApplicationException(content);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
