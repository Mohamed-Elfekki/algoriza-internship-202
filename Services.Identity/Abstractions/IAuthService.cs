using Application.DTOs.Credentials.Requests;
using Application.DTOs.Credentials.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Identity.Abstractions
{
	public interface IAuthService
	{
		Task<AuthDto> RegisterAsync(RegisterDto dto, string role);
		Task<AuthDto> LoginAsync(LoginDto dto);
		Task<AuthDto> CallRefreshTokenAsync(string token);
		Task<bool> RevokeTokenAsync(string token);
	}
}
