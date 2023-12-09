using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Respones
{
	public class BookingDtoRes
	{
		//public int BookingId { get; set; }
		public string? Image { get; set; }
		public string DoctorName { get; set; }
		public string Specialize { get; set; }
		public string Day { get; set; }
		public DateTime Time { get; set; }
		public double Price { get; set; }
		public string DiscountCode { get; set; }
		public double FinalPrice { get; set; }
		public string status { get; set; }
	}
}
