using Application.Repositories.Abstractions.Users;
using Domain.Entities.Credentials;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistance.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.Users
{
	public class PatientRepository : GenericRepository<Patient>, IPatientRepository
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public PatientRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base(context)
		{
			_context = context;
			_userManager = userManager;
		}

		public async Task<IEnumerable<Patient>> GetAllPatientsAsync(int requiredPage, int pageSize, Expression<Func<Patient, bool>>? filter)
		{
			decimal rowCount;
			if (filter == null)
			{
				rowCount = await _context.Patients.CountAsync();
			}
			else
			{
				rowCount = await _context.Patients.Where(filter).CountAsync();
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
			List<Patient> patients;
			if (filter == null)
			{
				patients = await _context.Patients.Include(x => x.User).Skip(skipCount).Take(pageSize).ToListAsync();
			}
			else
			{
				patients = await _context.Patients.Where(filter).Include(x => x.User).Skip(skipCount).Take(pageSize).ToListAsync();
			}
			if (patients.Count > 0)
			{
				return patients;
			}
			return null;
		}

		public async Task<int> GetNumOfPatientsAsync(string userId)
		{
			var isUserExist = await _userManager.FindByIdAsync(userId);
			if (isUserExist != null)
			{
				var isUserAnAdmin = await _context.Admins.Where(x => x.UserId == userId).SingleOrDefaultAsync();
				if (isUserAnAdmin != null)
				{
					var patients = await _context.Patients.CountAsync();
					return patients;
				}
			}
			return 0;
		}

		public async Task<Patient> GetPatientByIdAsync(string userId, int patientId)
		{
			var isUserExist = await _userManager.FindByIdAsync(userId);
			if (isUserExist != null)
			{
				var isUserAnAdmin = await _context.Admins.Where(x => x.UserId == userId).SingleOrDefaultAsync();
				if (isUserAnAdmin != null)
				{
					var patient = await _context.Patients.Where(x => x.PatientId == patientId).SingleOrDefaultAsync();
					if (patient != null)
					{
						var bookings = await _context.Bookings.Include(x => x.Coupon).Include(x => x.Time).Where(x => x.PatientId == patientId).ToListAsync();
						if (bookings.Count() > 0)
						{
							patient.Bookings = bookings;
							foreach (var booking in bookings)
							{
								var appointment = await _context.Appointments.Where(x => x.AppointmentId == booking.AppointmentId).SingleOrDefaultAsync();
								var coupon = await _context.Coupons.Where(x => x.CouponId == booking.CouponId).SingleOrDefaultAsync();
								if (appointment != null)
								{
									booking.Appointment = appointment;
									booking.Coupon = coupon;
									var doctor = await _context.Doctors.Include(x => x.User).Include(x => x.Specialization).Where(x => x.DoctorId == appointment.DoctorId).SingleOrDefaultAsync();
									if (doctor != null)
									{
										appointment.Doctor = doctor;
									}
								}

							}
							return patient;
						}
					}
				}
			}
			return null;
		}
	}
}
