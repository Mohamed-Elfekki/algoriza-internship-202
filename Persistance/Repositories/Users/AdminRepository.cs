using Application.Repositories.Abstractions.Users;
using Domain.Entities.Credentials;
using Persistance.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.Users
{
	public class AdminRepository : GenericRepository<Admin>, IAdminRepository
	{
		public AdminRepository(ApplicationDbContext context) : base(context)
		{
		}
	}
}
