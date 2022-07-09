using DatabaseLayer.Label;
using DatabaseLayer.Lable;
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
     public class LabelRL :ILabelRL
    {
        public readonly FundooContext fundooContext; //context class is used to query or save data to the database.
        public LabelRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }


        public async Task AddLabel(int userid, int noteid, string labelName)
        {
            try
            {

                var label1 = await fundooContext.Label.Where(c => c.UserId == userid && c.NoteId == noteid).FirstOrDefaultAsync();
                if (label1==null)
                {


                    Label label = new Label();

                    label.UserId = userid;
                    label.NoteId = noteid;
                    label.LabelName = labelName;
                    
                    await fundooContext.Label.AddAsync(label);
                    await fundooContext.SaveChangesAsync();
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public async Task<List<LabelResponseModel>> GetAllLabelsByLinqJoins(int UserId)
        {
            try
            {
                var label = fundooContext.Label.FirstOrDefault(u => u.UserId == UserId);
                if (label == null)
                {
                    return null;
                }


                 var res = await ( from user in fundooContext.Users 
                      join notes in fundooContext.Notes on user.UserId equals UserId
                      join labels in fundooContext.Label  on notes.NoteId equals labels.NoteId where labels.UserId == UserId


                 select new LabelResponseModel
                 {
                    UserId = UserId,
                    NoteId = notes.NoteId,
                    Title = notes.Title,
                    FirstName=user.FirstName,
                    LastName=user.LastName,
                    Email=user.Email, 
                    Description = notes.Description,
                    LabelName   = labels.LabelName,
                 }).ToListAsync();



                return res; 
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<LabelResponseModel>> GetlabelByNotesId(int UserId,int NoteId)
        {


            try
            {
                var label = fundooContext.Label.FirstOrDefault(u => u.UserId == UserId);
                if (label == null)
                {
                    return null;
                }


                var res = await (from user in fundooContext.Users
                                 join notes in fundooContext.Notes on user.UserId equals UserId where notes.NoteId == NoteId    
                                 join labels in fundooContext.Label on notes.NoteId equals labels.NoteId
                                 where labels.UserId == UserId


                                 select new LabelResponseModel
                                 {
                                     UserId = user.UserId,
                                     NoteId = notes.NoteId,
                                     Title = notes.Title,
                                     FirstName = user.FirstName,
                                     LastName = user.LastName,
                                     Email = user.Email,
                                     Description = notes.Description,
                                     LabelName = labels.LabelName,
                                 }).ToListAsync();



                return res;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            /*
               try
            {

                return await fundooContext.Label.Where(x => x.NoteId == NotesId && x.UserId==UserId).ToListAsync();
             
            }
            catch (Exception e)
            {
                throw e; 
            }
            */
        }
            

        public async Task<string> UpdateLabel(int UserId,int NoteId,string LabelName)
        {
            try
            {
                 var update = fundooContext.Label.Where(X => X.UserId== UserId && X.NoteId == NoteId).FirstOrDefault();
                 if (update != null)
                 {
                     update.LabelName =LabelName;
                     update.NoteId =   NoteId;

                     await fundooContext.SaveChangesAsync();
                     return "Label is modified";
                 }
                 else
                 {
                     return "Label is not modified";
                 }
                
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task<string> DeleteLabel(int UserId,int NoteId)
        {
            var deleteLabel = fundooContext.Label.Where(X => X.UserId == UserId && X.NoteId==NoteId).SingleOrDefault();
            if (deleteLabel != null)
            {
                fundooContext.Label.Remove(deleteLabel);
               await fundooContext.SaveChangesAsync();
                return "Label Deleted Successfully";
            }
            else
            {
                return null;
            }
         
        }
    }
}
