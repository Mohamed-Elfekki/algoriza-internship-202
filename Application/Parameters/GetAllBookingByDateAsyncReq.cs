using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Parameters
{
	public class GetAllBookingByDateAsyncReq
	{
		[Required]
		public int RequiredPage { get; set; }
		[Required]
		public int PageSize { get; set; }
		[Required]
		public DateTime Date { get; set; }
	}
}
