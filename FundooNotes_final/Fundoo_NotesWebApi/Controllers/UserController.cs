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
    }
}
