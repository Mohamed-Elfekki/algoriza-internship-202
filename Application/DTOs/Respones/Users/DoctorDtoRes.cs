using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Respones.Users
{
	public class DoctorDtoRes
	{
		public string? Image { get; set; }
		public string FullName { get; set; }
		public string Gender { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string SpecializationName { get; set; }
		public double Price { get; set; }
		public ICollection<AppointmentDtoRes> Appointments { get; set; }
	}
}
