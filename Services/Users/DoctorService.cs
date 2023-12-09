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
using System.Text;
using System.Threading.Tasks;

namespace Services.Users
{
	internal sealed class DoctorService : IDoctorService
	{
		private readonly IMapper _mapper;
		private readonly IAuthService _authService;
		private readonly IUnitOfWork _unitOfWork;

		public DoctorService(IUnitOfWork unitOfWork, IMapper mapper, IAuthService authService)
		{
			_mapper = mapper;
			_authService = authService;
			_unitOfWork = unitOfWork;
		}

		public async Task<AuthDto> AddDoctorAsync(AddDoctorDto dto)
		{
			var result = await _authService.RegisterAsync(dto, "Doctor");
			var specialization = await _unitOfWork.Specializations.GetByIdAsync(dto.specializeId);
			if (result != null)
			{
				Doctor doctor = new Doctor() { UserId = result.UserId, SpecializationId = specialization.SpecializationId, Price = dto.Price };
				var role = await _unitOfWork.Doctors.CreateAsync(doctor);
				if (role is true)
				{
					_unitOfWork.commit();
					return result;
				}
			}
			return result;
		}

		public async Task<bool> DeleteDoctorAsync(int doctorId)
		{
			var result = await _unitOfWork.Doctors.DeleteDoctorByIdAsync(doctorId);
			if (result == true)
			{
				_unitOfWork.commit();
				return true;
			}
			return false;
		}

		public async Task<bool> UpdateDoctorAsync(UpdateDoctorDto dto)
		{
			var result = _mapper.Map<Doctor>(dto);
			if (result != null)
			{
				var UpdatedDoctor = await _unitOfWork.Doctors.UpdateDoctorAsync(result);
				if (UpdatedDoctor is true)
				{
					_unitOfWork.commit();
					return true;
				}
			}
			return false;

		}


		public async Task<IEnumerable<DoctorDtoRes>> GetAllDoctorsAsync(int requiredPage, int pageSize, Expression<Func<Doctor, bool>>? filter)
		{
			var result = await _unitOfWork.Doctors.GetAllDoctorsAsync(requiredPage, pageSize, filter);
			if (result != null)
			{
				var doctors = _mapper.Map<ICollection<DoctorDtoRes>>(result);
				return doctors;
			}
			return null;
		}

		public async Task<DoctorDtoRes> GetDoctorByIdAsync(string userId, int patiendId)
		{
			var result = await _unitOfWork.Doctors.GetDoctorByIdAsync(userId, patiendId);
			var doctor = _mapper.Map<DoctorDtoRes>(result);
			if (doctor != null)
			{
				return doctor;
			}
			return null;
		}

		public async Task<int> GetNumOfDoctorsAsync(string userId)
		{
			var result = await _unitOfWork.Doctors.GetNumOfDoctorsAsync(userId);
			return result;
		}

		public async Task<List<Dictionary<string, object>>> GetTop10DoctorsAsync(string userId)
		{
			var result = await _unitOfWork.Doctors.GetTop10DoctorsAsync(userId);
			return result;
		}

	}
}
