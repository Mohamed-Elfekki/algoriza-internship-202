﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Credentials.Requests
{
	public class RevokeTokenDto
	{
		//? cookie || manual
		[Required]
		public string refreshToken { get; set; }
	}
}
