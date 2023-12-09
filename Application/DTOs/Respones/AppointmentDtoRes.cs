using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Respones
{
	public class AppointmentDtoRes
	{
		public int AppointmentId { get; set; }

		public string Day { get; set; }

		public ICollection<TimeDtoRes> Times { get; set; }
	}
}
