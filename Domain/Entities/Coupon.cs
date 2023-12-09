using Domain.Entities.Credentials;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
	public class Coupon
	{
		public int CouponId { get; set; }

		[Required]
		public string DisountCode { get; set; } = Guid.NewGuid().ToString();
		[Required]

		public Discount Discount { get; set; }
		[Required]
		public double DiscountValue { get; set; }
		[Required]
		public int CompletedRequests { get; set; }
		[Required]
		public bool Deactivate { get; set; } = false;




		public int AdminId { get; set; }
		public Admin Admin { get; set; }


		public ICollection<Booking> Bookings { get; set; }


	}
}
