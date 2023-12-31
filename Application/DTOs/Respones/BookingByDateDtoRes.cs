﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Respones
{
	public class BookingByDateDtoRes
	{
		public string PatientName { get; set; }

		public string Image { get; set; }

		public int Age { get; set; }
		public string Gender { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		public string AppointmentDay { get; set; }
		public DateTime AppointmentDateTime { get; set; }
	}
}
