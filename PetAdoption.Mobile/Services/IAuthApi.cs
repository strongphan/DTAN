using PetAdoption.Shared.Dtos;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetAdoption.Mobile.Services
{
    public interface IAuthApi
    {
        [Post("/api/Auth/login")]
        Task<Shared.Dtos.ApiResponse<AuthResponseDto>> LoginApi(LoginRequestDto dto);

        [Post("/api/Auth/register")]
        Task<Shared.Dtos.ApiResponse<AuthResponseDto>> RegisterApi(RegisterRequestDto dto);
    }

}
