using Application.DTOs.Requests;
using Application.Repositories.Abstractions.UnitOfWork;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Credentials;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
	internal sealed class AppointmentService : IAppointmentService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public AppointmentService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public async Task<string> AddAppointmentsAsync(string userId, AppointmentDtoReq dto)
		{
			var appointments = _mapper.Map<ICollection<Appointment>>(dto.Days);


			var result = await _unitOfWork.Appointments.AddAppointmentsAsync(userId, dto.Price, appointments);
			if (result == "Ok")
			{
				_unitOfWork.commit();
				return result;
			}
			return result;

		}

		public async Task<string> UpdateAnAppointmentAsync(string userId, UpdateAnAppointmentDtoReq dto)
		{
			var result = await _unitOfWork.Appointments.UpdateAnAppointmentAsync(userId, dto.TimeId, dto.DateTime);
			if (result == "Ok")
			{
				_unitOfWork.commit();
				return result;
			}
			return result;
		}

		public async Task<string> DeleteAnAppointmentAsync(string userId, int timeId)
		{
			var result = await _unitOfWork.Appointments.DeleteAnAppointmentAsync(userId, timeId);
			if (result == "Ok")
			{
				_unitOfWork.commit();
				return result;
			}
			return result;
		}


	}
}
