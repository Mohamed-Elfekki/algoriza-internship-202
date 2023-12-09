using Domain.Entities;
using Domain.Entities.Credentials;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Persistance.DataSeeding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.DataContext
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{

		public ApplicationDbContext(DbContextOptions options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelbuilder)
		{
			base.OnModelCreating(modelbuilder);
			new DataSeeder(modelbuilder).SeedRole();
			new DataSeeder(modelbuilder).SeedRelation();
		}


		public DbSet<Appointment> Appointments { get; set; }
		public DbSet<Booking> Bookings { get; set; }
		public DbSet<Coupon> Coupons { get; set; }
		public DbSet<Feedback> Feedbacks { get; set; }
		public DbSet<Specialization> Specializations { get; set; }
		public DbSet<Time> Times { get; set; }

		public DbSet<Doctor> Doctors { get; set; }
		public DbSet<Patient> Patients { get; set; }
		public DbSet<Admin> Admins { get; set; }
	}
}
