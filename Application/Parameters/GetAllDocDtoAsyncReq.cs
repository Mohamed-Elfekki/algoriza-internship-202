using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Parameters
{
	public class GetAllDocDtoAsyncReq
	{
		[Required]
		public int RequiredPage { get; set; }
		[Required]
		public int PageSize { get; set; }
		[AllowNull]
		public string? DoctorName { get; set; }
	}
}
