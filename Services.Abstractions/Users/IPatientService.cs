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
	public interface IPatientService
	{
		Task<int> GetNumOfPatientsAsync(string userId);
		Task<IEnumerable<PatientDtoRes>> GetAllPatientsAsync(int requiredPage, int pageSize, Expression<Func<Patient, bool>>? filter);

		Task<SinglePatientDtoRes> GetPatientByIdAsync(string userId, int patiendId);

		Task<AuthDto> RegisterPatientAsync(RegisterDto dto);
	}
}
