using Application.Repositories.Abstractions.UnitOfWork;
using AutoMapper;
using Domain.Entities.Credentials;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Persistance.DataContext;
using Services.Abstractions;
using Services.Abstractions.Users;
using Services.Identity;
using Services.Identity.Abstractions;
using Services.Identity.Settings;
using Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
	public sealed class ServiceManager : IServiceManager
	{
		private readonly Lazy<IDoctorService> _lazyDoctorService;
		private readonly Lazy<IPatientService> _lazyPatientService;
		private readonly Lazy<IBookingService> _lazyBookingService;
		private readonly Lazy<IAppointmentService> _lazyAppointmentService;
		private readonly Lazy<ISpecializationService> _lazySpecializationService;
		private readonly Lazy<ICouponService> _lazyCouponService;

		public ServiceManager(ApplicationDbContext context, IMapper mapper, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IAuthService authService)
		{
			_lazyDoctorService = new Lazy<IDoctorService>(() => new DoctorService(unitOfWork, mapper, authService));
			_lazyPatientService = new Lazy<IPatientService>(() => new PatientService(unitOfWork, mapper, userManager, authService));
			_lazyBookingService = new Lazy<IBookingService>(() => new BookingService(unitOfWork, userManager, mapper));
			_lazyAppointmentService = new Lazy<IAppointmentService>(() => new AppointmentService(unitOfWork, mapper));
			_lazySpecializationService = new Lazy<ISpecializationService>(() => new SpecializationService(unitOfWork));
			_lazyCouponService = new Lazy<ICouponService>(() => new CouponService(unitOfWork));
		}

		public IDoctorService DoctorService => _lazyDoctorService.Value;
		public IPatientService PatientService => _lazyPatientService.Value;
		public IBookingService BookingService => _lazyBookingService.Value;
		public IAppointmentService AppointmentService => _lazyAppointmentService.Value;
		public ISpecializationService SpecializationService => _lazySpecializationService.Value;
		public ICouponService CouponService => _lazyCouponService.Value;
	}
}
