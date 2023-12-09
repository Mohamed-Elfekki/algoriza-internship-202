using Domain.Entities.Credentials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
	public class Feedback
	{
		public int PatientId { get; set; }
		public Patient Patient { get; set; }
		public int DoctorId { get; set; }
		public Doctor Doctor { get; set; }

		public string Description { get; set; }
	}
}
