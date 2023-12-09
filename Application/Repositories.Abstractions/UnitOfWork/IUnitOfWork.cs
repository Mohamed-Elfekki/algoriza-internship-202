using Application.Repositories.Abstractions.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories.Abstractions.UnitOfWork
{
	public interface IUnitOfWork : IDisposable
	{
		IBookingRepository Bookings { get; }
		IAdminRepository Admins { get; }
		IDoctorRepository Doctors { get; }
		IPatientRepository Patients { get; }
		IAppointmentRepository Appointments { get; }
		ITimeRepository Times { get; }
		ISpecializationRepository Specializations { get; }
		ICouponRepository Coupons { get; }
		int commit();
	}
}
