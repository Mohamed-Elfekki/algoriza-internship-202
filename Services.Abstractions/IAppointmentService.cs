using Application.DTOs.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
	public interface IAppointmentService
	{
		Task<string> AddAppointmentsAsync(string userId, AppointmentDtoReq dto);
		Task<string> UpdateAnAppointmentAsync(string userId, UpdateAnAppointmentDtoReq dto);
		Task<string> DeleteAnAppointmentAsync(string userId, int timeId);
	}
}
