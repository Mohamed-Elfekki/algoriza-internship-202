using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Credentials
{
	public class Admin
	{
		public int AdminId { get; set; }
		public string UserId { get; set; }
		public ApplicationUser User { get; set; }
		public ICollection<Specialization> Specializations { get; set; }
		public ICollection<Coupon> Coupons { get; set; }

	}
}
