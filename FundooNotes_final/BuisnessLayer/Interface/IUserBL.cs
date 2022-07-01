﻿using DatabaseLayer.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessLayer.Interface
{
    public interface IUserBL
    {
        public void AddUser(UserPostModel userPostModel);
        public string LogInUser(string Email, string Password);


        public bool ForgotPassword(String Email);

    }
}
