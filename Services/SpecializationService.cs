using Application.Repositories.Abstractions.UnitOfWork;
using AutoMapper;
using Domain.Entities.Credentials;
using Microsoft.AspNetCore.Identity;
using Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
	internal sealed class SpecializationService : ISpecializationService
	{
		private readonly IUnitOfWork _unitOfWork;


		public SpecializationService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<IEnumerable<KeyValuePair<string, int>>> GetTo5SpecailizationsAsync(string userId)
		{
			var result = await _unitOfWork.Specializations.GetTo5SpecailizationsAsync(userId);
			return result;
		}
	}
}
