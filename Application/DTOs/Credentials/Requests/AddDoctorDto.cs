using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Credentials.Requests
{
	public class AddDoctorDto : RegisterDto
	{
		[Required]
		public int specializeId { get; set; }
		[Required]
		public double Price { get; set; }
	}
}
