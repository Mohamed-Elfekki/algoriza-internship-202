using Application.Repositories.Abstractions;
using Application.Repositories.Abstractions.UnitOfWork;
using Application.Repositories.Abstractions.Users;
using Domain.Entities.Credentials;
using Microsoft.AspNetCore.Identity;
using Persistance.DataContext;
using Persistance.Repositories.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public IBookingRepository Bookings { get; set; }
		public IAdminRepository Admins { get; set; }
		public IDoctorRepository Doctors { get; set; }
		public IPatientRepository Patients { get; set; }
		public IAppointmentRepository Appointments { get; set; }
		public ITimeRepository Times { get; set; }
		public ISpecializationRepository Specializations { get; set; }
		public ICouponRepository Coupons { get; set; }
		public UnitOfWork(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
			Bookings = new BookingRepository(_context, _userManager);
			Admins = new AdminRepository(_context);
			Doctors = new DoctorRepository(_context, userManager);
			Patients = new PatientRepository(_context, userManager);
			Appointments = new AppointmentRepository(_context, _userManager);
			Times = new TimeRepository(_context);
			Specializations = new SpecializationRepository(_context, userManager);
			Coupons = new CouponRepository(_context, userManager);
		}

		public int commit()
		{
			return _context.SaveChanges();
		}

		public void Dispose()
		{
			_context.Dispose();
		}
	}
}
