using Application.DTOs.Credentials.Requests;
using Application.DTOs.Requests;
using Application.DTOs.Respones;
using Application.Parameters;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Services.Identity.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Doctor")]
	public class DoctorController : ControllerBase
	{

		private readonly IServiceManager _serviceManager;
		private readonly IAuthService _authService;

		public DoctorController(IServiceManager serviceManager, IAuthService authService)
		{
			_serviceManager = serviceManager;
			_authService = authService;
		}

		#region Login
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

		[AllowAnonymous]
		[HttpPost("Login(RefreshToken)Async")]
		public async Task<IActionResult> DoctorLoginAsync([FromBody] LoginDto dto)
		{
			var result = await _authService.LoginAsync(dto);

			if (result.IsAuthenticated)
			{
				if (!string.IsNullOrEmpty(result.RefreshToken))
					setRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
				return Ok(result);
			}
			return BadRequest(result.Message);


		}
		#endregion

		#region Doctor

		[HttpPost("GetAllBookingByDateAsync")]
		public async Task<IActionResult> GetAllBookingByDateAsync(GetAllBookingByDateAsyncReq data)
		{
			string? userId = User.FindFirst("uid")?.Value;
			if (userId != null)
			{
				var result = await _serviceManager.BookingService.GetAllBookingByDateAsync(userId, data.RequiredPage, data.PageSize, data.Date);
				if (result != null)
				{
					return Ok(result);
				}
			}
			return BadRequest("No Data Found");
		}

		[HttpPut("ConfirmCheckUpAsync")]
		public async Task<IActionResult> ConfirmCheckUpAsync(int bookingId)
		{
			string? userId = User.FindFirst("uid")?.Value;
			if (userId != null)
			{
				var result = await _serviceManager.BookingService.ConfirmCheckUpAsync(userId, bookingId);
				if (result == "OK")
				{
					return Ok(true);
				}
			}
			return BadRequest(false);
		}


		[HttpPost("AddAppointmentsAsync")]
		public async Task<IActionResult> AddAppointmentsAsync([FromBody] AppointmentDtoReq dto)
		{
			if (ModelState.IsValid)
			{
				string? userId = User.FindFirst("uid")?.Value;
				if (userId != null)
				{
					var result = await _serviceManager.AppointmentService.AddAppointmentsAsync(userId, dto);
					if (result == "Ok")
					{
						return Ok(true);
					}
				}
			}
			return BadRequest(false);
		}

		[HttpPut("UpdateAnAppointmentAsync")]
		public async Task<IActionResult> UpdateAnAppointmentAsync([FromBody] UpdateAnAppointmentDtoReq dto)
		{
			if (ModelState.IsValid)
			{
				string? userId = User.FindFirst("uid")?.Value;
				if (userId != null)
				{
					var result = await _serviceManager.AppointmentService.UpdateAnAppointmentAsync(userId, dto);
					if (result == "Ok")
					{
						return Ok(true);
					}
				}
			}
			return BadRequest(false);
		}

		[HttpDelete("DeleteAnAppointmentAsync")]
		public async Task<IActionResult> DeleteAnAppointmentAsync([FromHeader, Required] int timeId)
		{
			string? userId = User.FindFirst("uid")?.Value;
			if (userId != null)
			{
				var result = await _serviceManager.AppointmentService.DeleteAnAppointmentAsync(userId, timeId);
				if (result == "Ok")
				{
					return Ok(true);
				}
			}
			return BadRequest(false);
		}
		#endregion
	}
}
