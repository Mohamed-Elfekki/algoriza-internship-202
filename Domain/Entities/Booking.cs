using Domain.Entities.Credentials;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
	public class Booking
	{
		public int BookingId { get; set; }

		[Required]
		public string Status { get; set; } = Enums.Status.Pending.ToString();



		public int PatientId { get; set; }
		public Patient Patient { get; set; }




		public int AppointmentId { get; set; }
		public Appointment Appointment { get; set; }

		public int TimeId { get; set; }
		public Time Time { get; set; }


		public int? CouponId { get; set; }
		public Coupon? Coupon { get; set; }

	}
}
