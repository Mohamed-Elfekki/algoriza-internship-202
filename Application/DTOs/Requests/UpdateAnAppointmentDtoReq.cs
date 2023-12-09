﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Requests
{
	public class UpdateAnAppointmentDtoReq
	{
		[Required]
		public int TimeId { get; set; }

		[Required]
		public DateTime DateTime { get; set; }


	}
}
