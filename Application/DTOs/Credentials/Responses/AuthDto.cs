using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Credentials.Responses
{
	public class AuthDto
	{
		public string UserId { get; set; }
		public string? Message { get; set; }
		public bool IsAuthenticated { get; set; }
		public string? Username { get; set; }
		public string? Email { get; set; }
		public string? Token { get; set; }
		public DateTime? ExpiresOn { get; set; }

		//[JsonIgnore]
		public string? RefreshToken { get; set; }

		public DateTime RefreshTokenExpiration { get; set; }
	}
}
