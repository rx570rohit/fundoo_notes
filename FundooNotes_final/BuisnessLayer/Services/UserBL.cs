using BuisnessLayer.Interface;
using DatabaseLayer.User;
using RepositoryLayer.Interfaces;

using RepositoryLayer.Services;
using RepositoryLayer.Services.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessLayer.Services
{
    public class UserBL : IUserBL
    {
        IUserRL userRL;
        public UserBL(IUserRL userRL)
        {
            this.userRL = userRL;
        }

        public void AddUser(UserPostModel userPostModel)
        {
            try
            {
                userRL.AddUser(userPostModel);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public string LogInUser(string Email, string Password)
        {
            try
            {
                return this.userRL.LogInUser(Email, Password);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public bool ForgotPassword(string Email)
        {
            try
            {
                return this.userRL.ForgotPassword(Email);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}


