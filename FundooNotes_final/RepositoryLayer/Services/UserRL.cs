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
using Experimental.System.Messaging;

namespace RepositoryLayer.Services
{
   public class UserRL : IUserRL   
    {
        FundooContext fundooContext;
        IConfiguration configuration;

        private readonly string _secret;


        public UserRL(FundooContext fundooContext, IConfiguration configuration)
        {
            this.fundooContext = fundooContext;

            this.configuration = configuration;
            this._secret = configuration.GetSection("JwtConfig").GetSection("SecretKey").Value;

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

      

        public bool ForgotPassword(string Email)
        {
            try
            {
                var user = fundooContext.Users.FirstOrDefault(u => u.Email == Email);
                if (user == null)
                {
                    return false;
                }
                else
                {
                    MessageQueue queue;
              
                    if (MessageQueue.Exists(@".\Private$\FundooQueue"))
                    {
                        queue = new MessageQueue(@".\Private$\FundooQueue");
                    }
                    else
                    {
                        queue = MessageQueue.Create(@".\Private$\FundooQueue");
                    }

                    Message MyMessage = new Message();
                    MyMessage.Formatter = new BinaryMessageFormatter();
                    MyMessage.Body = GenerateJwtToken(Email, user.UserId);
                    MyMessage.Label = "Forget Password Email";
                    queue.Send(MyMessage);

                    Message msg = queue.Receive();
                    msg.Formatter = new BinaryMessageFormatter();
                    EmailService.SendEmail(Email, msg.Body.ToString());
                    queue.ReceiveCompleted += new ReceiveCompletedEventHandler(msmqQueue_ReceiveCompleted);
                    queue.BeginReceive();
                    queue.Close();
                    return true;

                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private void msmqQueue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            try
            {
                MessageQueue queue = (MessageQueue)sender;
                Message msg = queue.EndReceive(e.AsyncResult);
                EmailService.SendEmail(e.Message.ToString(), GenerateToken(e.Message.ToString()));
                queue.BeginReceive();
            }
            catch (MessageQueueException ex)
            {
                if (ex.MessageQueueErrorCode ==
                   MessageQueueErrorCode.AccessDenied)
                {
                    Console.WriteLine("Access is denied. " +
                        "Queue might be a system queue.");
                }
            }
        }

        private string GenerateJwtToken(string email, int userId)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(this._secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new Claim(ClaimTypes.Email, email),
                    new Claim("UserId", userId.ToString())
                    }),

                    Expires = DateTime.UtcNow.AddMinutes(15),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string GenerateToken(string Email )
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this._secret);
            var tokenDiscriptor =new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email,Email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
        };
            var token =tokenHandler.CreateToken(tokenDiscriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool ResetPassword(string email, UserPasswordModel userPasswordModel)
        {
            try
            {
                var user = fundooContext.Users.Where(u => u.Email == email).FirstOrDefault();

                if (user == null)
                {
                    return false;
                }
                if (userPasswordModel.Password == userPasswordModel.ConfirmPassword)
                {
                    user.Password = PwdEncryptDecryptService.EncryptPassword(userPasswordModel.Password);
                    fundooContext.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
