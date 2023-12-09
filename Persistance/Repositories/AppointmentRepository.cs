using Application.DTOs.Requests;
using Application.Repositories.Abstractions;
using Application.Repositories.Abstractions.UnitOfWork;
using Domain.Entities;
using Domain.Entities.Credentials;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistance.DataContext;
using Persistance.Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
	public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public AppointmentRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base(context)
		{
			_context = context;
			_userManager = userManager;
		}

		public async Task<string> AddAppointmentsAsync(string userId, double Price, ICollection<Appointment> models)
		{

			var isDoctorExist = await _context.Doctors.Where(x => x.UserId == userId).SingleOrDefaultAsync();
			if (isDoctorExist != null)
			{
				foreach (var item in models)
				{
					item.DoctorId = isDoctorExist.DoctorId;
				}
				isDoctorExist.Price = Price;
				var updatedDoctor = _context.Doctors.Update(isDoctorExist);
				if (updatedDoctor != null)
				{
					await _context.Appointments.AddRangeAsync(models);
					return "Ok";
				}
				return "Failed To Add Price";
			}

			return "Failed To Create New Appointment";
		}

		public async Task<string> UpdateAnAppointmentAsync(string userId, int timeId, DateTime dateTime)
		{
			var isDoctorExist = await _context.Doctors.Where(x => x.UserId == userId).SingleOrDefaultAsync();
			if (isDoctorExist != null)
			{
				var isTimeExist = await _context.Times.FindAsync(timeId);
				if (isTimeExist != null)
				{
					var status = _context.Bookings.Where(x => x.AppointmentId == isTimeExist.AppointmentId && x.TimeId == timeId).Select(x => x.Status).SingleOrDefault();
					if (status != "Completed" && status != "Pending" && isTimeExist.DateTime != dateTime)
					{
						isTimeExist.DateTime = dateTime;
						_context.Times.Update(isTimeExist);
						return "Ok";
					}
					return "There are same Time, Completed Or Pending Bookings already";
				}
				return "Time Not Found";
			}
			return "User Not Found";
		}

		public async Task<string> DeleteAnAppointmentAsync(string userId, int timeId)
		{
			var isDoctorExist = await _context.Doctors.Where(x => x.UserId == userId).SingleOrDefaultAsync();
			if (isDoctorExist != null)
			{
				var isTimeExist = await _context.Times.FindAsync(timeId);
				if (isTimeExist != null)
				{
					var status = _context.Bookings.Where(x => x.AppointmentId == isTimeExist.AppointmentId && x.TimeId == timeId).Select(x => x.Status).SingleOrDefault();
					var bookings = await _context.Bookings.Where(x => x.TimeId == timeId).ToListAsync();
					if (status != "Completed" && status != "Pending")
					{
						_context.Bookings.RemoveRange(bookings);
						_context.Times.Remove(isTimeExist);
						return "Ok";
					}
					return "There are Pending Or Completed Bookings already";
				}
				return "Time Not Found";
			}
			return "User Not Found";
		}
	}
}
