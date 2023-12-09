using Services.Abstractions.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
	public interface IServiceManager
	{
		IDoctorService DoctorService { get; }
		IPatientService PatientService { get; }
		IBookingService BookingService { get; }
		IAppointmentService AppointmentService { get; }
		ISpecializationService SpecializationService { get; }
		ICouponService CouponService { get; }

	}
}
