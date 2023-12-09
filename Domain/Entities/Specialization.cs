using Domain.Entities.Credentials;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
	public class Specialization
	{
		public int SpecializationId { get; set; }
		[Required, MaxLength(30)]
		public string SpecializationName { get; set; }


		public int AdminId { get; set; }
		public Admin Admin { get; set; }


		public ICollection<Doctor> Doctors { get; set; }
	}
}
