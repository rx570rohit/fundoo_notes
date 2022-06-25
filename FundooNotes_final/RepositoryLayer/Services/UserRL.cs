using DatabaseLayer.User;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Services
{
   public class UserRL : IUserRL  
    {


        FundooContext fundooContext;
        IConfiguration configuration;
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
                user.FirstName=userPostModel.FirstName;
                user.LastName=userPostModel.LastName;
                user.Email=userPostModel.Email; 
                user.Password=userPostModel.Password;
                user.CreatedDate = DateTime.Now;
                user.ModifiedDate = DateTime.Now;
                fundooContext.Add(user);
                fundooContext.SaveChanges();
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
