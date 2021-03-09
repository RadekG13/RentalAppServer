using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RentalApp3.Models;
using RentalApp3.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RentalApp3.Services
{
    public interface IUserService
    {
        Task<Response2> RegisterUserAsync(RegisterViewModel model);
        Task<Response2> LoginUserAsync(LoginViewModel model);
    }

    public class UserService : IUserService
    {
        private UserManager<AppUser> _userManager;

        private IConfiguration _configuration;
        

        public UserService(UserManager<AppUser> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _configuration = configuration;
           
        }

        public async Task<Response2> RegisterUserAsync(RegisterViewModel model)
        {
            if (model == null)
                throw new NullReferenceException("Register Model is null");

            if (model.Password != model.ConfirmPassword)
                return new Response2
                {
                    Message = "Wrong confirm password",
                    IsSuccess = false,

                };

            var identityUser = new AppUser
            {
                Email = model.Email,
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber,
                NameAndSurname = model.NameAndSurname,
                AccountNumber = model.AccountNumber
            };

            var result = await _userManager.CreateAsync(identityUser, model.Password); //Password hashed
            if (result.Succeeded)
            {
                return new Response2
                {
                    Message = "User created!",
                    IsSuccess = true,
                };
            }
            IEnumerable<string> err = result.Errors.Select(e => e.Description);
            string addmessage = "";
            if (err.Contains("Username '" + identityUser.Email + "' is already taken."))
            {
                addmessage = "dodać obsługę drugiego konta?"; //zmiana username
            }
            string allErrors = string.Join(", ", err);
            return new Response2
            {
                Message = "User did not create:",
                IsSuccess = false,

            };
        }


        public async Task<Response2> LoginUserAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email); //Sprawdzić email==name??
            

            if (user == null)
            {
                return new Response2
                {
                    Message = "User doesn't exist",
                    IsSuccess = false,

                };
            }

            var result = await _userManager.CheckPasswordAsync(user, model.Password);


     
            if (!result)
                return new Response2
                {
                    Message = "Wrong password",
                    IsSuccess = false,
                };

            var claims = new Claim[]
              {
                new Claim("Email", model.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id), //Tutaj sprawdzić czy bedzie działać
              };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthKey"]));

            var token = new JwtSecurityToken(
               claims: claims,
               expires: DateTime.Now.AddDays(1),
               signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );
            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);


      


            return new Response2
            {
                Message = "Bearer " + tokenAsString,
                IsSuccess = true,
                //  ExpireDate = token.ValidTo
            };

        }





    }
}
