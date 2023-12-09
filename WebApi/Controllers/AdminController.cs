using Application.DTOs.Credentials.Requests;
using Application.DTOs.Requests;
using Application.Parameters;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Services.Abstractions;
using Services.Identity.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Admin")]
	public class AdminController : ControllerBase
	{
		private readonly IServiceManager _serviceManager;
		private readonly IAuthService _authService;

		public AdminController(IServiceManager serviceManager, IAuthService authService)
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
		public async Task<IActionResult> AdminLoginAsync([FromBody] LoginDto dto)
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

		#region Statistics

		[HttpGet("Statistics/GetNumberOfDoctorsAsync")]
		public async Task<IActionResult> NumberOfDoctorsAsync()
		{
			string? userId = User.FindFirst("uid")?.Value;
			if (userId != null)
			{
				var doctors = await _serviceManager.DoctorService.GetNumOfDoctorsAsync(userId);
				return Ok(doctors);
			}
			return BadRequest("No Data Found");
		}


		[HttpGet("Statistics/GetNumberOfPatientsAsync")]
		public async Task<IActionResult> NumberOfPatientsAsync()
		{
			string? userId = User.FindFirst("uid")?.Value;
			if (userId != null)
			{
				var patients = await _serviceManager.PatientService.GetNumOfPatientsAsync(userId);
				return Ok(patients);
			}
			return BadRequest("No Data Found");
		}


		[HttpGet("Statistics/GetNumberOfRequestsAsync")]
		public async Task<IActionResult> NumberOfRequestsAsync()
		{
			string? userId = User.FindFirst("uid")?.Value;
			if (userId != null)
			{
				var Requests = await _serviceManager.BookingService.GetNumOfBookingsAsync(userId);
				return Ok(Requests);
			}
			return BadRequest("No Data Found");
		}



		[HttpGet("Statistics/GetTop5SpecializationsAsync")]
		public async Task<IActionResult> GetTop5SpecializationsAsync()
		{
			string? userId = User.FindFirst("uid")?.Value;
			if (userId != null)
			{
				var Requests = await _serviceManager.SpecializationService.GetTo5SpecailizationsAsync(userId);
				return Ok(Requests);
			}
			return BadRequest("No Data Found");
		}


		[HttpGet("Statistics/GetTop10DoctorsAsync")]
		public async Task<IActionResult> GetTop10DoctorsAsync()
		{
			string? userId = User.FindFirst("uid")?.Value;
			if (userId != null)
			{
				var Requests = await _serviceManager.DoctorService.GetTop10DoctorsAsync(userId);
				return Ok(Requests);
			}
			return BadRequest("No Data Found");
		}
		#endregion

		#region Doctors
		[HttpPost("Doctors/GetAllDoctors(Search-Optional)Async")]
		public async Task<IActionResult> GetAllDoctorsAsync(GetAllDocDtoAsyncReq data)
		{
			if (ModelState.IsValid)
			{
				var result = await _serviceManager.DoctorService.GetAllDoctorsAsync(data.RequiredPage, data.PageSize, x => x.User.FirstName.Contains(data.DoctorName));
				if (result == null)
				{
					return BadRequest("Data Not Found");
				}
				return Ok(result);
			}
			return BadRequest(ModelState);
		}

		[HttpGet("Doctors/GetDoctorByIdAsync")]
		public async Task<IActionResult> GetDoctorByIdAsync([FromHeader, Required] int doctorId)
		{
			string? userId = User.FindFirst("uid")?.Value;
			if (userId != null)
			{
				var result = await _serviceManager.DoctorService.GetDoctorByIdAsync(userId, doctorId);
				if (result != null)
				{
					return Ok(result);
				}
			}
			return BadRequest("No Data Found");
		}

		[HttpPost("Doctors/AddDoctorAsync")]
		public async Task<IActionResult> AddDoctorAsync([FromForm] AddDoctorDto dto)
		{
			if (ModelState.IsValid)
			{
				var result = await _serviceManager.DoctorService.AddDoctorAsync(dto);
				if (result != null)
				{
					return Ok(result);
				}
			}
			return BadRequest(ModelState);
		}

		[HttpPut("Doctors/EditDoctorAsync")]
		public async Task<IActionResult> EditDoctorByIdAsync([FromForm] UpdateDoctorDto dto)
		{
			if (ModelState.IsValid)
			{
				var result = await _serviceManager.DoctorService.UpdateDoctorAsync(dto);
				if (result == true)
				{
					return Ok(result);
				}
				return BadRequest(false);
			}
			return BadRequest(ModelState);
		}

		[HttpDelete("Doctors/DeleteDoctorByIdAsync")]
		public async Task<IActionResult> DeleteDoctorByIdAsync(int doctorId)
		{
			var result = await _serviceManager.DoctorService.DeleteDoctorAsync(doctorId);
			if (result == true)
			{
				return Ok(true);
			}
			return BadRequest(false);
		}


		#endregion

		#region Patients
		[HttpPost("Patients/GetAllPatients(Search-Optional)Async")]
		public async Task<IActionResult> GetAllPatientsAsync([FromBody] GetAllPatientsAsyncDto data)
		{
			var result = await _serviceManager.PatientService.GetAllPatientsAsync(data.RequiredPage, data.PageSize, x => x.User.FirstName.Contains(data.PatientName));
			if (result != null)
			{
				return Ok(result);
			}
			return BadRequest("No Data Found");
		}

		[HttpGet("Patients/GetPatientByIdAsync")]
		public async Task<IActionResult> GetPatientByIdAsync([FromHeader, Required] int patientId)
		{
			string? userId = User.FindFirst("uid")?.Value;
			if (userId != null)
			{
				var result = await _serviceManager.PatientService.GetPatientByIdAsync(userId, patientId);
				if (result != null)
				{
					return Ok(result);
				}
			}
			return BadRequest("No Data Found");
		}

		#endregion

		#region Coupon
		[HttpPost("Coupons/AddCouponAsync")]
		public async Task<IActionResult> AddCouponAsync([FromForm] CouponDtoReq dto)
		{
			if (ModelState.IsValid)
			{
				string? userId = User.FindFirst("uid")?.Value;
				if (userId != null)
				{
					var result = await _serviceManager.CouponService.AddCouponAsync(userId, dto);
					if (result is true)
					{
						return Ok(true);
					}
					return BadRequest(false);
				}
			}
			return BadRequest(ModelState);
		}

		[HttpPut("Coupons/UpdateCouponAsync")]
		public async Task<IActionResult> UpdateCouponAsync([FromHeader, Required] int couponId, [FromForm] CouponDtoReq dto)
		{
			if (ModelState.IsValid)
			{
				var result = await _serviceManager.CouponService.UpdateCouponAsync(couponId, dto);
				if (result is true)
				{
					return Ok(true);
				}
				return BadRequest(false);
			}
			return BadRequest(ModelState);
		}

		[HttpDelete("Coupons/DeleteCouponAsync")]
		public async Task<IActionResult> DeleteCouponAsync(int couponId)
		{
			var result = await _serviceManager.CouponService.DeleteCouponAsync(couponId);
			if (result is true)
			{
				return Ok(true);
			}
			return BadRequest(false);

		}

		[HttpPut("Coupons/DeactivateCouponAsync")]
		public async Task<IActionResult> DeactivateCouponAsync(int couponId)
		{
			var result = await _serviceManager.CouponService.DeactivateCouponAsync(couponId);
			if (result is true)
			{
				return Ok(true);
			}
			return BadRequest(false);

		}
		#endregion

	}
}
