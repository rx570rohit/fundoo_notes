using DatabaseLayer.User;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Services;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace RepositoryLayer.Services
{
   public class UserRL : IUserRL   
    {
        FundooContext fundooContext;
        IConfiguration configuration;

        public readonly String _secret;


        public UserRL(FundooContext fundooContext, IConfiguration configuration)
        {
            this.fundooContext = fundooContext;

            this.configuration = configuration;
        }
        public void AddUser(UserPostModel userPostModel)
        {
            try
            {
                User user = new User();
                user.FirstName = userPostModel.FirstName;
                user.LastName = userPostModel.LastName;
                user.Email = userPostModel.Email;
                user.Password = PwdEncryptDecryptService.EncryptPassword(userPostModel.Password);
                user.CreatedDate = DateTime.Now;
                user.ModifiedDate = DateTime.Now;
                fundooContext.Add(user);
                fundooContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string LogInUser(string email, string password)
        {
            try
            {
                var user = fundooContext.Users.Where(u=>u.Email==email).FirstOrDefault();
                if (user != null)
                {
                    string Password = PwdEncryptDecryptService.DecryptPassword(user.Password);
                    if (password == Password)
                    {
                      return GenerateJwtToken(email, user.UserId);
                    }
                    throw new Exception("Password is invalid");
                }
                throw new Exception("Email doesn't Exist");
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        private String GenerateJwtToken(String email, int userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email,email),
                    new Claim("UserID",userId.ToString()),
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        
      

    }
}
