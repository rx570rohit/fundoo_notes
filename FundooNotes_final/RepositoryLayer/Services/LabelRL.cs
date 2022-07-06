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
                if (label1 !=null)
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


        public async Task<IEnumerable<Label>> GetAllLabels(int UserId)
        {
            try
            {
                return await fundooContext.Label.Where(x=>x.UserId==UserId).ToListAsync();
                
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<Label>> GetlabelByNotesId(int UserId,int NotesId)
        {
            try
            {
                return await fundooContext.Label.Where(x => x.NoteId == NotesId && x.UserId==UserId).ToListAsync();
             
            }
            catch (Exception e)
            {
                throw e; ;
            }
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
