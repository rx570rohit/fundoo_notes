using DatabaseLayer.Collab;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
   public class CollabRL :ICollabRL
    {

        public readonly FundooContext fundooContext; //context class is used to query or save data to the database.

        public CollabRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }

   
        public async Task<bool> AddCollab(int userId,CollabModel collabModel)
        {
            try
            {
                var noteData = this.fundooContext.Notes.Where(x => x.NoteId == collabModel.NoteId).FirstOrDefault();
                var userData = this.fundooContext.Users.Where(x => x.UserId == userId).FirstOrDefault();
                if (noteData != null && userData != null)
                {
                    Collab collab = new Collab();
                    collab.CollabEmail = collabModel.CollabEmail;
                    collab.NoteId = collabModel.NoteId;
                    collab.UserId = userData.UserId;
                    this.fundooContext.collabs.Add(collab);
                }

               
                int result = await this.fundooContext.SaveChangesAsync();
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

      
        public async Task<IEnumerable<CollabResponseModel>> GetAllCollabs(int userId)
        {

            try
            {
                var label = fundooContext.collabs.FirstOrDefault(u => u.UserId == userId);
                if (label == null)
                {
                    return null;
                }


                var res = await (from user in fundooContext.Users
                                 join notes in fundooContext.Notes on user.UserId equals userId
                           //      join labels in fundooContext.Label on notes.NoteId equals labels.NoteId
                                 join Collabs in fundooContext.collabs on notes.NoteId equals Collabs.NoteId      
                                 where Collabs.UserId == userId


                                 select new CollabResponseModel
                                 {
                                     UserId = user.UserId,
                                     NoteId = notes.NoteId,
                                     Title = notes.Title,
                                     FirstName = user.FirstName,
                                     LastName = user.LastName,
                                     Email = user.Email,
                                     Description = notes.Description,
                                     CollabEmail=Collabs.CollabEmail

                                 }).ToListAsync();



                return res;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            /*  try
              {
                  var result = await this.fundooContext.collabs.Where(x=>x.UserId==userId).ToListAsync();
                  return  result;
              }
              catch (Exception e)
              {
                  throw new Exception(e.Message);
              }
            */
        }

        
         public async Task<IEnumerable<CollabResponseModel>> GetCollabsByNoteId(int userId, int noteId)
        {

            try
            {
                var label = fundooContext.Label.FirstOrDefault(u => u.UserId == userId && u.NoteId==noteId);
                if (label == null)
                {
                    return null;
                }


                var res = await (from user in fundooContext.Users
                                 join notes in fundooContext.Notes on user.UserId equals userId
                                 where notes.NoteId == noteId
                                 // join labels in fundooContext.Label on notes.NoteId equals labels.NoteId
                                 join Collabs in fundooContext.collabs on notes.NoteId equals Collabs.NoteId
                                 where Collabs.UserId == userId


                                 select new CollabResponseModel
                                 {
                                     UserId = user.UserId,
                                     NoteId = notes.NoteId,
                                     Title = notes.Title,
                                     FirstName = user.FirstName,
                                     LastName = user.LastName,
                                     Email = user.Email,
                                     Description = notes.Description,
                                     CollabEmail = Collabs.CollabEmail

                                 }).ToListAsync();



                return res;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            /* try
            {
                var result =   await this.fundooContext.collabs.Where(x => x.NoteId == noteId &&x.UserId==userId).ToListAsync();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }*/
        }

       
        public async Task<string> ReomoveCollab(int userId , int noteId)
        {
            var collab = fundooContext.collabs.Where(X => X.NoteId==noteId && X.UserId==userId).SingleOrDefault();
            if (collab != null)
            {
                fundooContext.collabs.Remove(collab);
              await this.fundooContext.SaveChangesAsync();
                return "Member removed from collaboration Successfully";
            }
            else
            {
                return null;
            }
        }
    }
}
