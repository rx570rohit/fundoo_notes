using BuisnessLayer.Interface;
using DatabaseLayer.User;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Services;
using RepositoryLayer;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Collections.Generic;
using RepositoryLayer.Services.Entities;
using System.Text;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace Fundoo_NotesWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        IUserBL userBL;
        FundooContext fundooContext;
       private readonly IDistributedCache distributedCache;

        public UserController(IUserBL userBL, FundooContext fundooContext , IDistributedCache distributedCache)
        {
            this.userBL = userBL;
            this.fundooContext = fundooContext;
            this.distributedCache = distributedCache;
        }
        [HttpPost("Register")]
        public IActionResult AddUser(UserPostModel userPostModel)
        {
            try
            {

                var user = fundooContext.Users.FirstOrDefault(u => u.Email == userPostModel.Email);
                if (user != null)
                {
                    return this.BadRequest(new { success = false, message = "Email Already Exits" });

                }
                this.userBL.AddUser(userPostModel);
                return this.Ok(new { success = true, message = "Registration Successfull" });


            }
            catch (Exception e)
            {
                throw e;
            }

        }

        [HttpGet("GetAllUsersUsingRedisCache")]
        public async Task<IActionResult> GetAllUsersUsingRedisCache()
        {
            var cacheKey = "UsersList";
            string serializedUsersList;
            var usersList = new List<User>();
            var redisUsersList = await this.distributedCache.GetAsync(cacheKey);
            if (redisUsersList != null)
            {
                serializedUsersList = Encoding.UTF8.GetString(redisUsersList);
                usersList = JsonConvert.DeserializeObject<List<User>>(serializedUsersList);
            }
            else
            {
                usersList = await this.fundooContext.Users.ToListAsync();  
                serializedUsersList = JsonConvert.SerializeObject(usersList);
                redisUsersList = Encoding.UTF8.GetBytes(serializedUsersList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await this.distributedCache.SetAsync(cacheKey, redisUsersList, options);
            }
            

            return this.Ok(new { status = 200, isSuccess = true, message = "All user are loaded", data = usersList });

            //return this.Ok(usersList);
        }

        [HttpPost("LogInEmailPassword/{Email}/{Password}")]

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
        [HttpPost("Forgotpassword/{Email}")]
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
            return this.Ok(new { success = true, message = "Tokne sented successfully to respective email Id ", data = result });
        }

        [Authorize]
        [HttpPut("Resetpassword")]
        public IActionResult Resetpassword(UserPasswordModel userPasswordModel)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserID = Int32.Parse(userid.Value);
                var result = fundooContext.Users.Where(u => u.UserId == UserID).FirstOrDefault();
                string Email = result.Email.ToString();
                if (userPasswordModel.Password != userPasswordModel.ConfirmPassword)
                {
                    return BadRequest(new { success = false, message = "Password and Confirm password must be same" });
                }
                bool res = this.userBL.ResetPassword(Email, userPasswordModel);
                if (res == false)
                {
                    return this.BadRequest(new { sucess = false, message = "Enter the valid Email" });
                }
                return this.Ok(new { succes = true, message = "Password change successfully" });
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}
