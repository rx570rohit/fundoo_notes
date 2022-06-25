using DatabaseLayer.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessLayer.Interface
{
    public interface IUserBL
    {

        public void AddUser(UserPostModel userPostModel);
       
    }
}
