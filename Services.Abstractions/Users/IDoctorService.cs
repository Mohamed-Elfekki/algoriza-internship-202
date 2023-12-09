using Application.DTOs.Credentials.Requests;
using Application.DTOs.Credentials.Responses;
using Application.DTOs.Respones;
using Application.DTOs.Respones.Users;
using Domain.Entities.Credentials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions.Users
{
	public interface IDoctorService
	{
		Task<AuthDto> AddDoctorAsync(AddDoctorDto dto);
		Task<bool> UpdateDoctorAsync(UpdateDoctorDto dto);
		Task<bool> DeleteDoctorAsync(int doctorId);
		Task<IEnumerable<DoctorDtoRes>> GetAllDoctorsAsync(int requiredPage, int pageSize, Expression<Func<Doctor, bool>>? filter);
		Task<DoctorDtoRes> GetDoctorByIdAsync(string userId, int patiendId);
		Task<int> GetNumOfDoctorsAsync(string userId);

		Task<List<Dictionary<string, object>>> GetTop10DoctorsAsync(string userId);
	}
}
