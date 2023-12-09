using Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTOs.Credentials.Requests
{
	public class RegisterDto
	{
		[Required, StringLength(100)]
		public string FirstName { get; set; }

		[Required, StringLength(100)]
		public string LastName { get; set; }

		[Required, StringLength(50)]
		public string Username { get; set; }

		[EmailAddress, Required, StringLength(128)]
		public string Email { get; set; }

		[StringLength(256)]
		public string Password { get; set; }

		[Phone]
		public string PhoneNumber { get; set; }

		[Required, Range(0, 1)]
		public Gender Gender { get; set; }

		[Required, DataType(DataType.Date)]
		public DateTime DateOfBirth { get; set; }

		public IFormFile? File { get; set; }

	}
}
