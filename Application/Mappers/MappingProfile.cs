using Application.DTOs.Credentials.Requests;
using Application.DTOs.Requests;
using Application.DTOs.Respones;
using Application.DTOs.Respones.Users;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Credentials;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			//============================================================================================================================
			CreateMap<Doctor, DoctorDtoRes>()
				.ForMember(dest => dest.FullName, opt => opt.MapFrom(src => (src.User.FirstName + src.User.LastName)))
				 .ForMember(dest => dest.SpecializationName, opt => opt.MapFrom(src => (src.Specialization.SpecializationName)))
				  .ForMember(dest => dest.Price, opt => opt.MapFrom(src => (src.Price)))
				  .ForMember(dest => dest.Image, opt => opt.MapFrom(src => (src.User.HasedFilePath)))
				  .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => (src.User.Gender)))
				  .ForMember(dest => dest.Email, opt => opt.MapFrom(src => (src.User.Email)))
				  .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => (src.User.PhoneNumber)));

			CreateMap<Appointment, AppointmentDtoRes>()
				.ForMember(dest => dest.Times, opt => opt.MapFrom(src => src.Times));

			CreateMap<Time, TimeDtoRes>();
			//============================================================================================================================
			CreateMap<Booking, BookingDtoRes>()
					.ForMember(dest => dest.Image, opt => opt.MapFrom(src => (src.Appointment.Doctor.User.HasedFilePath)))
					.ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => (src.Appointment.Doctor.User.FirstName + src.Appointment.Doctor.User.LastName)))
					.ForMember(dest => dest.Specialize, opt => opt.MapFrom(src => (src.Appointment.Doctor.Specialization.SpecializationName)))
					.ForMember(dest => dest.Day, opt => opt.MapFrom(src => (src.Appointment.Day)))
					 .ForMember(dest => dest.Time, opt => opt.MapFrom(src => (src.Time.DateTime)))
					  .ForMember(dest => dest.Price, opt => opt.MapFrom(src => (src.Appointment.Doctor.Price)))
					   .ForMember(dest => dest.DiscountCode, opt => opt.MapFrom(src => (src.Coupon.DisountCode)))
					   .ForMember(dest => dest.FinalPrice, opt =>
					   opt.MapFrom(src => (src.Coupon.Discount == Domain.Enums.Discount.Value ? src.Appointment.Doctor.Price - src.Coupon.DiscountValue : src.Appointment.Doctor.Price - ((src.Appointment.Doctor.Price * src.Coupon.DiscountValue) / 100))))
					   .ForMember(dest => dest.status, opt => opt.MapFrom(src => (src.Status)));
			//=================================================================================================================
			CreateMap<Booking, BookingByDateDtoRes>()
					.ForMember(dest => dest.Image, opt => opt.MapFrom(src => (src.Patient.User.HasedFilePath)))
					.ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => (src.Patient.User.FirstName + src.Patient.User.LastName)))
					.ForMember(dest => dest.Age, opt => opt.MapFrom(src => ((DateTime.UtcNow.Year) - (src.Patient.User.DateOfBirth.Year))))
					.ForMember(dest => dest.Gender, opt => opt.MapFrom(src => (src.Patient.User.Gender)))
					 .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => (src.Patient.User.PhoneNumber)))
					  .ForMember(dest => dest.Email, opt => opt.MapFrom(src => (src.Patient.User.Email)))
					   .ForMember(dest => dest.AppointmentDay, opt => opt.MapFrom(src => (src.Appointment.Day)))
					   .ForMember(dest => dest.AppointmentDateTime, opt => opt.MapFrom(src => (src.Time.DateTime)));

			//=================================================================================================================

			CreateMap<DayDtoReq, Appointment>()
			.ForMember(dest => dest.Day, opt => opt.MapFrom(src => (src.Day)));
			//.ForPath(dest => dest.Times.Select(x => x.DateTime), opt => opt.MapFrom(src => (src.Times.Select(x => x.DateTime))));
			CreateMap<TimeDtoReq, Time>()
				.ForMember(dest => dest.DateTime, opt => opt.MapFrom(src => src.DateTime));

			//=================================================================================================================

			CreateMap<Patient, PatientDtoRes>()
				.ForMember(x => x.Image, opt => opt.MapFrom(x => x.User.HasedFilePath))
					.ForMember(x => x.FullName, opt => opt.MapFrom(x => x.User.FirstName + " " + x.User.LastName))
					.ForMember(x => x.Email, opt => opt.MapFrom(x => x.User.Email))
					.ForMember(x => x.Phone, opt => opt.MapFrom(x => x.User.PhoneNumber))
					.ForMember(x => x.Gender, opt => opt.MapFrom(x => x.User.Gender))
					.ForMember(x => x.BirthOfDate, opt => opt.MapFrom(x => x.User.DateOfBirth));
			//====================================================================================================================
			CreateMap<Patient, SinglePatientDtoRes>()
					.ForMember(x => x.Image, opt => opt.MapFrom(x => x.User.HasedFilePath))
					.ForMember(x => x.FullName, opt => opt.MapFrom(x => x.User.FirstName + " " + x.User.LastName))
					.ForMember(x => x.Email, opt => opt.MapFrom(x => x.User.Email))
					.ForMember(x => x.Phone, opt => opt.MapFrom(x => x.User.PhoneNumber))
					.ForMember(x => x.Gender, opt => opt.MapFrom(x => x.User.Gender))
					.ForMember(x => x.BirthOfDate, opt => opt.MapFrom(x => x.User.DateOfBirth));
			//====================================================================================================================


			CreateMap<UpdateDoctorDto, Doctor>()
				.ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
				.ForMember(dest => dest.SpecializationId, opt => opt.MapFrom(src => src.specializeId))
				.ForPath(dest => dest.User.FirstName, opt => opt.MapFrom(src => src.FirstName))
				.ForPath(dest => dest.User.LastName, opt => opt.MapFrom(src => src.LastName))
				.ForPath(dest => dest.User.UserName, opt => opt.MapFrom(src => src.Username))
				.ForPath(dest => dest.User.Gender, opt => opt.MapFrom(src => src.Gender))
				.ForPath(dest => dest.User.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth));
		}

	}
}
