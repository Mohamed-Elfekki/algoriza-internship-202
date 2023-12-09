using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Credentials
{
	public class Doctor
	{
		public int DoctorId { get; set; }

		public string UserId { get; set; }
		public ApplicationUser User { get; set; }


		public ICollection<Appointment> Appointments { get; set; }
		public ICollection<Feedback> Feedbacks { get; set; }


		public int SpecializationId { get; set; }
		public Specialization Specialization { get; set; }

		[Required]
		public double Price { get; set; }

	}
}
