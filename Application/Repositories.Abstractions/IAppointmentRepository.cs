using Application.DTOs.Requests;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories.Abstractions
{
	public interface IAppointmentRepository : IGenericRepository<Appointment>
	{
		Task<string> AddAppointmentsAsync(string userId, double Price, ICollection<Appointment> models);
		Task<string> UpdateAnAppointmentAsync(string userId, int timeId, DateTime dateTime);
		Task<string> DeleteAnAppointmentAsync(string userId, int timeId);
	}
}
