using BuisnessLayer.Interface;

using DatabaseLayer.User;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Services;
using RepositoryLayer;
using System;
using System.Linq;



namespace Fundoo_NotesWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        IUserBL userBL;
        FundooContext fundooContext;

        public UserController(IUserBL userBL, FundooContext fundooContext)
        {
            this.userBL = userBL;
            this.fundooContext = fundooContext;
        }
        [HttpPost("Register")]
        public IActionResult AddUser(UserPostModel userPostModel)
        {
            try
            {
                this.userBL.AddUser(userPostModel);
                var user = fundooContext.Users.FirstOrDefault(u => u.Email == userPostModel.Email);
                if(user != null)
                {
                    return this.Ok(new { success = true, message = "Registration Successfull" });
                }
               
                return this.BadRequest(new { success = false, message = "Email Already Exits" });
            }
            catch(Exception e)
            {
                throw e;
            }
        }
        [HttpPost("LogIn")]

        public IActionResult LogIn(String Email,String Password)
        {
            try
            {
                var user = fundooContext.Users.FirstOrDefault(u => u.Email == Email);
                

                if (user == null)
                {
                    return this.BadRequest(new { success = false, message = "Email doesn't Exits" });
                }

                string password = PwdEncryptDecryptService.DecryptPassword(user.Password);

                var userdata1 = fundooContext.Users.FirstOrDefault(u => u.Email == Email && password == Password);
                if (userdata1 == null)
                {
                    return this.BadRequest(new { success = false, message = "Password is Invalid" });
                }

                string token = this.userBL.LogInUser(Email, Password);

                return this.Ok(new { success = true, message = "LogIn Successfull", data = token });
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }
        [HttpPost("ForgotPassword")]
        public IActionResult ForgotPassword(String Email)
        {
            var user = fundooContext.Users.FirstOrDefault(u => u.Email == Email);
            if (user == null)
            {
                return this.BadRequest(new {success =false,message = "Email Not Found" });
            }
            string Password = PwdEncryptDecryptService.DecryptPassword(user.Password);

            var userdata1 = fundooContext.Users.FirstOrDefault(u => u.Email == Email);
            if (userdata1 == null)
            {
                return this.BadRequest(new { success = false, message = "Password " });
            }
            bool result = this.userBL.ForgotPassword(Email);
            return this.Ok(new { success = true, message = "LogIn Successfull", data = result });
        }

    }
}
