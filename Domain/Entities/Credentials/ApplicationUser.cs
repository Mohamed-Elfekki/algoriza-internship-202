using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities.Credentials
{
	public class ApplicationUser : IdentityUser
	{
		[Required, MaxLength(25)]
		public string FirstName { get; set; }

		[Required, MaxLength(25)]
		public string LastName { get; set; }

		[Required]
		public string Gender { get; set; }

		[Required, DataType(DataType.Date)]
		public DateTime DateOfBirth { get; set; }

		public string? FilePath { get; set; }

		public string? HasedFilePath { get; set; }



		public List<RefreshToken>? RefreshTokens { get; set; }
		public Doctor? Doctor { get; set; }
		public Patient? Patient { get; set; }
		public Admin? Admin { get; set; }
	}
}
