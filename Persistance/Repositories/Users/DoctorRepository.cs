
using Application.DTOs.Respones.Users;
using Application.Repositories.Abstractions.Users;
using Domain.Entities.Credentials;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistance.DataContext;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.Users
{
	public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public DoctorRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base(context)
		{
			_context = context;
			_userManager = userManager;
		}


		public async Task<bool> UpdateDoctorAsync(Doctor data)
		{
			var isDoctorExist = await _context.Doctors.Include(x => x.User).Where(x => x.DoctorId == data.DoctorId).FirstOrDefaultAsync();
			if (isDoctorExist != null)
			{
				isDoctorExist.Price = data.Price;
				isDoctorExist.SpecializationId = data.SpecializationId;
				isDoctorExist.User.FirstName = data.User.FirstName;
				isDoctorExist.User.LastName = data.User.LastName;
				isDoctorExist.User.UserName = data.User.UserName;
				isDoctorExist.User.DateOfBirth = data.User.DateOfBirth;
				isDoctorExist.User.Gender = data.User.Gender;
				await _userManager.UpdateAsync(isDoctorExist.User);
				_context.Doctors.Update(isDoctorExist);
				return true;
			}
			return false;
		}

		public async Task<bool> DeleteDoctorByIdAsync(int doctorId)
		{
			var doctor = await _context.Doctors.Include(x => x.Appointments).Where(x => x.DoctorId == doctorId).FirstOrDefaultAsync();
			if (doctor != null)
			{
				var apps = await _context.Appointments.Where(x => x.DoctorId == doctorId).ToListAsync();
				foreach (var item in apps)
				{
					var Booking = await _context.Bookings.Where(x => x.AppointmentId == item.AppointmentId).ToListAsync();
					if (Booking.Count() > 0)
					{
						return false;
					}
				}
				_context.Doctors.Remove(doctor);
				return true;
			}
			return false;
		}

		public async Task<IEnumerable<Doctor>> GetAllDoctorsAsync(int requiredPage, int pageSize, Expression<Func<Doctor, bool>>? filter)
		{
			decimal rowCount;
			if (filter == null)
			{
				rowCount = await _context.Doctors.Include(x => x.User).CountAsync();
			}
			else
			{
				rowCount = await _context.Doctors.Include(x => x.User).Where(filter).CountAsync();
			}
			if (pageSize <= 0)
			{
				pageSize = 1;
			}
			var pagesCount = Math.Ceiling(rowCount / pageSize);
			if (requiredPage > pagesCount)
			{
				requiredPage = 1;
			}
			if (requiredPage <= 0)
			{
				requiredPage = 1;
			}

			int skipCount = (requiredPage - 1) * pageSize;
			List<Doctor> result;
			if (filter == null)
			{
				result = await _context.Doctors.Include(x => x.Specialization).Include(x => x.User).Skip(skipCount).Take(pageSize).ToListAsync();
			}
			else
			{
				result = await _context.Doctors.Include(x => x.Specialization).Include(x => x.User).Where(filter).Skip(skipCount).Take(pageSize).ToListAsync();
			}
			foreach (var item in result)
			{
				var app = await _context.Appointments.Where(x => x.DoctorId == item.DoctorId).ToListAsync();
				item.Appointments = app;
				foreach (var item2 in app)
				{
					var time = await _context.Times.Where(x => x.AppointmentId == item2.AppointmentId).ToListAsync();
					item2.Times = time;
				}
			}
			if (result.Count > 0)
			{
				return result;
			}
			return null;
		}

		public async Task<Doctor> GetDoctorByIdAsync(string userId)
		{
			var doctor = await _context.Doctors.Where(x => x.UserId == userId).SingleOrDefaultAsync();
			if (doctor != null)
			{
				return doctor;
			}
			return null;
		}

		public async Task<Doctor> GetDoctorByIdAsync(string userId, int doctorId)
		{
			var isUserExist = await _userManager.FindByIdAsync(userId);
			if (isUserExist != null)
			{
				var isUserAnAdmin = await _context.Admins.Where(x => x.UserId == userId).SingleOrDefaultAsync();
				if (isUserAnAdmin != null)
				{
					var doctor = await _context.Doctors.Include(x => x.User).Include(x => x.Appointments).Include(x => x.Specialization).Where(x => x.DoctorId == doctorId).SingleOrDefaultAsync();
					if (doctor != null)
					{
						var app = await _context.Appointments.Where(x => x.DoctorId == doctor.DoctorId).ToListAsync();
						doctor.Appointments = app;
						foreach (var item2 in app)
						{
							var time = await _context.Times.Where(x => x.AppointmentId == item2.AppointmentId).ToListAsync();
							item2.Times = time;
						}
						return doctor;
					}
				}
			}
			return null;
		}

		public async Task<int> GetNumOfDoctorsAsync(string userId)
		{
			var isUserExist = await _userManager.FindByIdAsync(userId);
			if (isUserExist != null)
			{
				var isUserAnAdmin = await _context.Admins.Where(x => x.UserId == userId).SingleOrDefaultAsync();
				if (isUserAnAdmin != null)
				{
					var doctors = await _context.Doctors.CountAsync();
					return doctors;
				}
			}
			return 0;
		}

		//public async Task<List<Dictionary<string, object>>> GetTop10DoctorsAsync(string userId)
		//{
		//	var isUserExist = await _userManager.FindByIdAsync(userId);
		//	if (isUserExist != null)
		//	{
		//		var isUserAnAdmin = await _context.Admins.Where(x => x.UserId == userId).SingleOrDefaultAsync();
		//		if (isUserAnAdmin != null)
		//		{

		//			List<Dictionary<string, object>> keyValues = new List<Dictionary<string, object>>();
		//			var apps = await _context.Bookings.Select(x => x.AppointmentId).Distinct().ToListAsync();
		//			foreach (var app in apps)
		//			{
		//				Dictionary<string, object> result = new Dictionary<string, object>();
		//				var numOfBookings = await _context.Bookings.Where(x => x.AppointmentId == app).CountAsync();
		//				var doctorId = await _context.Appointments.Where(x => x.AppointmentId == app).Select(x => x.DoctorId).SingleOrDefaultAsync();
		//				var doctor = await _context.Doctors.Include(x => x.Specialization).Include(x => x.User).Where(x => x.DoctorId == doctorId).SingleOrDefaultAsync();

		//				result.Add("Image", doctor.User.HasedFilePath);
		//				result.Add("FullName", doctor.User.FirstName + " " + doctor.User.LastName);
		//				result.Add("Specialization", doctor.Specialization.SpecializationName);
		//				result.Add("#Requests", numOfBookings);
		//				keyValues.Add(result);
		//			}
		//			return keyValues;
		//		}
		//	}
		//	return null;
		//}
		public async Task<List<Dictionary<string, object>>> GetTop10DoctorsAsync(string userId)
		{
			var isUserExist = await _userManager.FindByIdAsync(userId);
			if (isUserExist != null)
			{
				var isUserAnAdmin = await _context.Admins.Where(x => x.UserId == userId).SingleOrDefaultAsync();
				if (isUserAnAdmin != null)
				{
					var results = await _context.Appointments
						.Include(x => x.Doctor.User)
						.Include(x => x.Doctor.Specialization)
						.Include(x => x.Bookings)
						.GroupBy(x => new { x.DoctorId, x.Doctor.User.HasedFilePath, x.Doctor.User.FirstName, x.Doctor.User.LastName, x.Doctor.Specialization.SpecializationName })
						.Select(g => new
						{
							DoctorId = g.Key.DoctorId,
							Image = g.Key.HasedFilePath,
							FullName = g.Key.FirstName + " " + g.Key.LastName,
							Specialization = g.Key.SpecializationName,
							NumBookings = g.Count()
						})
						.OrderByDescending(x => x.NumBookings)
						.Take(10)
						.ToListAsync();

					return results.Select(x => new Dictionary<string, object>
					{
						{ "Image", x.Image },
						{ "FullName", x.FullName },
						{ "Specialization", x.Specialization },
						{ "#Requests", x.NumBookings }
					}).ToList();
				}
			}
			return null;
		}

	}
}
