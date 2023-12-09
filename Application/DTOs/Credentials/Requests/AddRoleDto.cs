﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Credentials.Requests
{
	public class AddRoleDto
	{
		[Required]
		public string UserId { get; set; }
		[Required]
		public string Role { get; set; }
	}
}
