using Application.DTOs.Respones.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Respones
{
	public class SinglePatientDtoRes
	{
		public string Image { get; set; }
		public string FullName { get; set; }

		public string Email { get; set; }

		public string Phone { get; set; }

		public string Gender { get; set; }

		public DateTime BirthOfDate { get; set; }
		public ICollection<BookingDtoRes> Bookings { get; set; }
	}
}
