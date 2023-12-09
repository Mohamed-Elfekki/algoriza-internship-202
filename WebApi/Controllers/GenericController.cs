using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Services.Identity.Abstractions;

namespace WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles ="Admin"), Authorize(Roles ="Doctor"), Authorize(Roles ="Patient")]
	public class GenericController : ControllerBase
	{
		private readonly IAuthService _authService;

		public GenericController(IAuthService authService)
		{
			_authService = authService;
		}

		private void setRefreshTokenInCookie(string refreshToken, DateTime expires)
		{
			var cookieOptions = new CookieOptions
			{
				HttpOnly = true,
				Expires = expires.ToLocalTime(),
				Secure = true,
				IsEssential = true,
				SameSite = SameSiteMode.None
			};

			Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
		}

		[HttpPost("PassRefreshTokenToCatchJWTAsync")]
		public async Task<IActionResult> PassRefreshTokenToCatchJWTAsync(string refreshToken)
		{
			var result = await _authService.CallRefreshTokenAsync(refreshToken);
			if (result.IsAuthenticated)
			{
				if (!string.IsNullOrEmpty(result.RefreshToken))
					setRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
				return Ok(result);
			}
			return BadRequest(result.Message);
		}

	}
}
