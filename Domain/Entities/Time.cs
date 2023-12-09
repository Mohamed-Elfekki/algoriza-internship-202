using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
	public class Time
	{
		public int TimeId { get; set; }

		public DateTime DateTime { get; set; }


		public int AppointmentId { get; set; }
		public Appointment Appointment { get; set; }

		public ICollection<Booking> Bookings { get; set; }
	}
}
