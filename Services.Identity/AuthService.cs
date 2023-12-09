using Application.DTOs.Credentials.Requests;
using Application.DTOs.Credentials.Responses;
using Application.Repositories.Abstractions.UnitOfWork;
using Azure;
using Domain.Entities.Credentials;
using Domain.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Identity.Abstractions;
using Services.Identity.Settings;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.Identity
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IWebHostEnvironment _web;
		private readonly JWT _jwt;

		public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt, IWebHostEnvironment web)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_web = web;
			_jwt = jwt.Value;
		}

		public async Task<AuthDto> RegisterAsync(RegisterDto model, string role)
		{
			if (await _userManager.FindByEmailAsync(model.Email) is not null)
				return new AuthDto { Message = "Email is already registered!" };

			if (await _userManager.FindByNameAsync(model.Username) is not null)
				return new AuthDto { Message = "Username is already registered!" };


			var newName = Guid.NewGuid().ToString();
			var FilePath = model.File.FileName;
			var HasedFilePath = string.Concat(newName, Path.GetExtension(FilePath));
			var root = _web.WebRootPath;
			var path = Path.Combine(root, "Uploads/Users", HasedFilePath);
			using (var fs = System.IO.File.Create(path))
			{
				await model.File.CopyToAsync(fs);
			}

			var user = new ApplicationUser
			{
				UserName = model.Username,
				Email = model.Email,
				FirstName = model.FirstName,
				LastName = model.LastName,
				Gender = model.Gender.ToString(),
				DateOfBirth = model.DateOfBirth,
				PhoneNumber = model.PhoneNumber,
				FilePath = FilePath,
				HasedFilePath = HasedFilePath,
			};

			var result = await _userManager.CreateAsync(user, model.Password);

			if (!result.Succeeded)
			{
				var errors = string.Empty;

				foreach (var error in result.Errors)
					errors += $"{error.Description},";

				return new AuthDto { Message = errors };
			}

			//Default Role
			await _userManager.AddToRoleAsync(user, "User");
			//Specific Role
			await _userManager.AddToRoleAsync(user, role);


			var jwtSecurityToken = await CreateJwtToken(user);

			var refreshToken = GenerateRefreshToken();
			user.RefreshTokens?.Add(refreshToken);
			await _userManager.UpdateAsync(user);

			return new AuthDto
			{
				UserId = user.Id,
				Email = user.Email,
				ExpiresOn = jwtSecurityToken.ValidTo,
				IsAuthenticated = true,
				Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
				Username = user.UserName,
				RefreshToken = refreshToken.Token,
				RefreshTokenExpiration = refreshToken.ExpiresOn
			};
		}

		public async Task<AuthDto> LoginAsync(LoginDto model)
		{
			var authDto = new AuthDto();

			var user = await _userManager.FindByEmailAsync(model.Email);

			if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
			{
				authDto.Message = "Email or Password is incorrect!";
				return authDto;
			}

			var jwtSecurityToken = await CreateJwtToken(user);
			authDto.UserId = user.Id;
			authDto.IsAuthenticated = true;
			authDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
			authDto.Email = user.Email;
			authDto.Username = user.UserName;
			authDto.ExpiresOn = jwtSecurityToken.ValidTo;

			if (user.RefreshTokens.Any(t => t.IsActive))
			{
				var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
				authDto.RefreshToken = activeRefreshToken.Token;
				authDto.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
			}
			else
			{
				var refreshToken = GenerateRefreshToken();
				authDto.RefreshToken = refreshToken.Token;
				authDto.RefreshTokenExpiration = refreshToken.ExpiresOn;
				user.RefreshTokens.Add(refreshToken);
				await _userManager.UpdateAsync(user);
			}

			return authDto;
		}

		public async Task<AuthDto> CallRefreshTokenAsync(string token)
		{
			var authModel = new AuthDto();

			var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

			if (user == null)
			{
				authModel.Message = "Invalid token";
				return authModel;
			}

			var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

			if (!refreshToken.IsActive)
			{
				authModel.Message = "Inactive token";
				return authModel;
			}

			refreshToken.RevokedOn = DateTime.UtcNow;

			var newRefreshToken = GenerateRefreshToken();
			user.RefreshTokens.Add(newRefreshToken);
			await _userManager.UpdateAsync(user);

			var jwtToken = await CreateJwtToken(user);
			authModel.IsAuthenticated = true;
			authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
			authModel.Email = user.Email;
			authModel.Username = user.UserName;
			authModel.RefreshToken = newRefreshToken.Token;
			authModel.RefreshTokenExpiration = newRefreshToken.ExpiresOn;

			return authModel;
		}

		public async Task<bool> RevokeTokenAsync(string token)
		{
			var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

			if (user == null)
				return false;

			var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

			if (!refreshToken.IsActive)
				return false;

			refreshToken.RevokedOn = DateTime.UtcNow;

			await _userManager.UpdateAsync(user);

			return true;
		}

		private RefreshToken GenerateRefreshToken()
		{
			var randomNumber = new byte[32];
			var generator = RandomNumberGenerator.Create();
			//using var generator = new RNGCryptoServiceProvider();
			generator.GetBytes(randomNumber);

			return new RefreshToken
			{
				Token = Convert.ToBase64String(randomNumber),
				ExpiresOn = DateTime.UtcNow.AddDays(5),
				CreatedOn = DateTime.UtcNow
			};
		}
		private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
		{
			var userClaims = await _userManager.GetClaimsAsync(user);
			var roles = await _userManager.GetRolesAsync(user);
			var roleClaims = new List<Claim>();

			foreach (var role in roles)
				roleClaims.Add(new Claim("roles", role));

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim("uid", user.Id)
			}
			.Union(userClaims)
			.Union(roleClaims);

			var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
			var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

			var jwtSecurityToken = new JwtSecurityToken(
				issuer: _jwt.Issuer,
				audience: _jwt.Audience,
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
				signingCredentials: signingCredentials);

			return jwtSecurityToken;
		}
	}
}
