using Domain.Entities;
using Domain.Entities.Credentials;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.DataSeeding
{
	public class DataSeeder
	{
		private readonly ModelBuilder modelBuilder;

		public DataSeeder(ModelBuilder modelBuilder)
		{
			this.modelBuilder = modelBuilder;
		}


		public void SeedRelation()
		{

			//Required 1:1 ApplicationUser, Patient
			modelBuilder.Entity<ApplicationUser>()
				.HasOne(e => e.Patient)
				.WithOne(e => e.User)
				.HasForeignKey<Patient>(e => e.UserId)
				//.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();

			//Required 1:1 ApplicationUser, Admin
			modelBuilder.Entity<ApplicationUser>()
				.HasOne(e => e.Doctor)
				.WithOne(e => e.User)
				.HasForeignKey<Doctor>(e => e.UserId)
				//.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();

			//Required 1:1 ApplicationUser, Admin
			modelBuilder.Entity<ApplicationUser>()
				.HasOne(e => e.Admin)
				.WithOne(e => e.User)
				.HasForeignKey<Admin>(e => e.UserId)
				//.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();

			//Required 1:M Booking, Appointment
			modelBuilder.Entity<Appointment>()
				.HasMany(e => e.Bookings)
				.WithOne(e => e.Appointment)
				.HasForeignKey(e => e.AppointmentId)
				//.OnDelete(DeleteBehavior.Restrict)
				.IsRequired();

			//Required 1:M Booking, Time
			modelBuilder.Entity<Time>()
				.HasMany(e => e.Bookings)
				.WithOne(e => e.Time)
				.HasForeignKey(e => e.TimeId)
				.OnDelete(DeleteBehavior.Restrict)
				.IsRequired();

			//Required 1:M Appointment,Time
			modelBuilder.Entity<Appointment>()
				.HasMany(e => e.Times)
				.WithOne(e => e.Appointment)
				.HasForeignKey(e => e.AppointmentId)
				//.OnDelete(DeleteBehavior.Restrict)
				.IsRequired();

			//Optional 1:M Booking, Coupon
			modelBuilder.Entity<Coupon>()
				.HasMany(e => e.Bookings)
				.WithOne(e => e.Coupon)
				.HasForeignKey(e => e.CouponId)
				//.OnDelete(DeleteBehavior.SetNull)
				.IsRequired(false);

			//Required 1:M Patient, Booking
			modelBuilder.Entity<Patient>()
				.HasMany(e => e.Bookings)
				.WithOne(e => e.Patient)
				.HasForeignKey(e => e.PatientId)
				.OnDelete(DeleteBehavior.Restrict)
				.IsRequired();

			//Required 1:M Admin, Coupon
			modelBuilder.Entity<Admin>()
				.HasMany(e => e.Coupons)
				.WithOne(e => e.Admin)
				.HasForeignKey(e => e.AdminId)
				//.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();

			//Required 1:M Admin, Specialization
			modelBuilder.Entity<Admin>()
				.HasMany(e => e.Specializations)
				.WithOne(e => e.Admin)
				.HasForeignKey(e => e.AdminId)
				//.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();

			//Required 1:M Doctor, Appointment
			modelBuilder.Entity<Doctor>()
				.HasMany(e => e.Appointments)
				.WithOne(e => e.Doctor)
				.HasForeignKey(e => e.DoctorId)
				//.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();

			//Required 1:M Doctor, Specialization
			modelBuilder.Entity<Specialization>()
				.HasMany(e => e.Doctors)
				.WithOne(e => e.Specialization)
				.HasForeignKey(e => e.SpecializationId)
				.OnDelete(DeleteBehavior.Restrict)
				.IsRequired();

			//Optional M:M Feedack (Patient -> Doctor)
			modelBuilder.Entity<Feedback>()
				.HasKey(x => new { x.PatientId, x.DoctorId });

			modelBuilder.Entity<Feedback>()
				.HasOne(e => e.Doctor)
				.WithMany(e => e.Feedbacks)
				.HasForeignKey(e => e.DoctorId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Feedback>()
				.HasOne(e => e.Patient)
				.WithMany(e => e.Feedbacks)
				.HasForeignKey(e => e.PatientId)
				.OnDelete(DeleteBehavior.Restrict);

		}

		public void SeedRole()
		{
			modelBuilder.Entity<IdentityRole>().HasData(
		   new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "User", NormalizedName = "User".ToUpper(), ConcurrencyStamp = Guid.NewGuid().ToString() },
				new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Admin", NormalizedName = "Admin".ToUpper(), ConcurrencyStamp = Guid.NewGuid().ToString() },
				new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Doctor", NormalizedName = "Doctor".ToUpper(), ConcurrencyStamp = Guid.NewGuid().ToString() },
				new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Patient", NormalizedName = "Patient".ToUpper(), ConcurrencyStamp = Guid.NewGuid().ToString() }
				);
		}

	}
}
