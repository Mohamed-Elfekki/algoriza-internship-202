using Domain.Entities.Credentials;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
	public class Appointment
	{
		public int AppointmentId { get; set; }

		[Required]
		public string Day { get; set; }


		public int DoctorId { get; set; }
		public Doctor Doctor { get; set; }



		public ICollection<Booking> Bookings { get; set; }
		public ICollection<Time> Times { get; set; }


	}
}
