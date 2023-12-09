using Application.DTOs.Respones.Users;
using Domain.Entities.Credentials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories.Abstractions.Users
{
	public interface IDoctorRepository : IGenericRepository<Doctor>
	{
		Task<IEnumerable<Doctor>> GetAllDoctorsAsync(int requiredPage, int pageSize, Expression<Func<Doctor, bool>>? filter);
		Task<Doctor> GetDoctorByIdAsync(string userId);
		Task<Doctor> GetDoctorByIdAsync(string userId, int doctorId);
		Task<int> GetNumOfDoctorsAsync(string userId);
		Task<bool> UpdateDoctorAsync(Doctor doctor);
		Task<bool> DeleteDoctorByIdAsync(int doctorId);

		Task<List<Dictionary<string, object>>> GetTop10DoctorsAsync(string userId);

	}
}
