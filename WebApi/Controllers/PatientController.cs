using Application.DTOs.Credentials.Requests;
using Application.DTOs.Credentials.Responses;
using Application.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Services.Identity.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Patient")]
	public class PatientController : ControllerBase
	{
		private readonly IServiceManager _serviceManager;
		private readonly IAuthService _authService;

		public PatientController(IServiceManager serviceManager, IAuthService authService)
		{
			_serviceManager = serviceManager;
			_authService = authService;
		}

		#region Register - Login

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
		[HttpPost("Register(RefreshToken)Async")]
		public async Task<IActionResult> PatientRegisterAsync([FromForm] RegisterDto dto)
		{
			if (ModelState.IsValid)
			{
				var result = await _serviceManager.PatientService.RegisterPatientAsync(dto);
				if (result != null)
				{
					if (!string.IsNullOrEmpty(result.RefreshToken))
						setRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
					return Ok(result);
				}
			}
			return BadRequest(ModelState);
		}

		[AllowAnonymous]
		[HttpPost("Login(RefreshToken)Async")]
		public async Task<IActionResult> PatientLoginAsync([FromBody] LoginDto dto)
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

		#region Patient

		[HttpPost("GetAllDoctors(Search-Optional)Async")]
		public async Task<IActionResult> GetAllDoctorsAsync(GetAllDocDtoAsyncReq data)
		{
			if (ModelState.IsValid)
			{
				var result = await _serviceManager.DoctorService.GetAllDoctorsAsync(data.RequiredPage, data.PageSize, x => x.User.FirstName.Contains(data.DoctorName));
				if (result == null)
				{
					return NotFound("Data Not Found");
				}
				return Ok(result);
			}
			return BadRequest(ModelState);
		}

		[HttpPost("CreateBookingAsync")]
		public async Task<IActionResult> CreateBookingAsync(CreateBookingAsyncReq data)
		{
			if (ModelState.IsValid)
			{
				string? userId = User.FindFirst("uid")?.Value;
				if (userId != null)
				{
					var result = await _serviceManager.BookingService.CreateBookingAsync(userId, data.AppointmentId, data.TimeId, data.CouponCode);
					if (result == "Ok")
					{
						return Ok(true);
					}
				}
			}
			return BadRequest(false);

		}

		[HttpGet("GetAllBookingAsync")]
		public async Task<IActionResult> GetAllBookingAsync()
		{
			string? userId = User.FindFirst("uid")?.Value;
			if (userId != null)
			{
				var result = await _serviceManager.BookingService.GetAllBookingByIdAsync(userId);
				if (result != null)
				{
					return Ok(result);
				}
			}
			return BadRequest("No Data Found");
		}

		[HttpPut("CancelBookingAsync")]
		public async Task<IActionResult> CancelBookingAsync([Required] int bookingId)
		{
			string? userId = User.FindFirst("uid")?.Value;
			if (userId != null)
			{
				var result = await _serviceManager.BookingService.CancelBookingAsync(userId, bookingId);
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
