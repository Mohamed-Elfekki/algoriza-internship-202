using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Credentials
{
	public class Patient
	{
		public int PatientId { get; set; }

		public string UserId { get; set; }
		public ApplicationUser User { get; set; }
		public ICollection<Booking> Bookings { get; set; }
		public ICollection<Feedback> Feedbacks { get; set; }
	}
}
