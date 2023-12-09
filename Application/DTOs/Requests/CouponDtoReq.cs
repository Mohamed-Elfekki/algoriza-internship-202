using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Requests
{
	public class CouponDtoReq
	{
		[Required]
		public Discount Discount { get; set; }
		[Required]
		public double DiscountValue { get; set; }
	}
}
