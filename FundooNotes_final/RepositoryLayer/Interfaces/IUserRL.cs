using DatabaseLayer.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
   public interface IUserRL
    {
        public void AddUser(UserPostModel userPostModel);
        public string LogInUser(String userName ,String Password);

        public bool ForgotPassword(String Email);
        bool ResetPassword(string email, UserPasswordModel userPasswordModel);
    }
}
