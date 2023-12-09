using Application.DTOs.Credentials.Requests;
using Application.DTOs.Credentials.Responses;
using Application.DTOs.Respones;
using Application.DTOs.Respones.Users;
using Application.Repositories.Abstractions.UnitOfWork;
using AutoMapper;
using Domain.Entities.Credentials;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistance.DataContext;
using Services.Abstractions.Users;
using Services.Identity.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Services.Users
{
	internal sealed class PatientService : IPatientService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IAuthService _authService;

		public PatientService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager, IAuthService authService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_userManager = userManager;
			_authService = authService;
		}


		public async Task<AuthDto> RegisterPatientAsync(RegisterDto dto)
		{
			var result = await _authService.RegisterAsync(dto, "Patient");
			if (result != null)
			{
				Patient patient = new Patient() { UserId = result.UserId };
				var role = await _unitOfWork.Patients.CreateAsync(patient);
				if (role is true)
				{
					_unitOfWork.commit();
					return result;
				}
			}
			return result;
		}

		public async Task<IEnumerable<PatientDtoRes>> GetAllPatientsAsync(int requiredPage, int pageSize, Expression<Func<Patient, bool>>? filter)
		{
			var patients = await _unitOfWork.Patients.GetAllPatientsAsync(requiredPage, pageSize, filter);
			if (patients != null)
			{
				var result = _mapper.Map<IEnumerable<PatientDtoRes>>(patients);
				if (result != null)
				{
					return result;
				}
				return null;
			}
			return null;
		}

		public async Task<int> GetNumOfPatientsAsync(string userId)
		{
			var result = await _unitOfWork.Patients.GetNumOfPatientsAsync(userId);
			return result;
		}

		public async Task<SinglePatientDtoRes> GetPatientByIdAsync(string userId, int patiendId)
		{
			var result = await _unitOfWork.Patients.GetPatientByIdAsync(userId, patiendId);
			var patient = _mapper.Map<SinglePatientDtoRes>(result);
			if (patient != null)
			{
				return patient;
			}
			return null;
		}

	}
}
