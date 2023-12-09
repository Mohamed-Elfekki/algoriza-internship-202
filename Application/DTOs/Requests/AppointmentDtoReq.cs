using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTOs.Requests
{
	public class AppointmentDtoReq
	{
		public double Price { get; set; }
		public ICollection<DayDtoReq> Days { get; set; }
	}

	public class DayDtoReq
	{
		public Days Day { get; set; }
		public ICollection<TimeDtoReq> Times { get; set; }

	}

	public class TimeDtoReq
	{
		public DateTime DateTime { get; set; }
	}
}
