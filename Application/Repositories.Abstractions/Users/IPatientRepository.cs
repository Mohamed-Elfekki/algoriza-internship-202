using Application.DTOs.Respones.Users;
using Domain.Entities;
using Domain.Entities.Credentials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories.Abstractions.Users
{
	public interface IPatientRepository : IGenericRepository<Patient>
	{
		Task<int> GetNumOfPatientsAsync(string userId);
		Task<IEnumerable<Patient>> GetAllPatientsAsync(int requiredPage, int pageSize, Expression<Func<Patient, bool>>? filter);
		Task<Patient> GetPatientByIdAsync(string userId, int patiendId);
	}
}
