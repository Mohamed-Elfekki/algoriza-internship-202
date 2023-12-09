using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Parameters
{
	public class CreateBookingAsyncReq
	{
		//[Required]
		//public string UserId { get; set; }
		[Required]
		public int AppointmentId { get; set; }
		[Required]
		public int TimeId { get; set; }
		[AllowNull]
		public string? CouponCode { get; set; }
	}
}
