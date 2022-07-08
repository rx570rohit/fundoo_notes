using BuisnessLayer.Interface;
using DatabaseLayer.Label;
using DatabaseLayer.Lable;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer.Services
{
    public class LabelBL : ILabelBL
    {
        ILabelRL labelRL;
        public LabelBL(ILabelRL labelRL)
        {
            this.labelRL = labelRL;
        }

        public async Task AddLabel(int UserId, int NoteId, string labelName)
        {
            try
            {
                await labelRL.AddLabel(UserId,NoteId,labelName);   
            }
            catch (Exception e)
            {

                throw e;
            }
        }

       

        public async Task <IEnumerable<Label>> GetAllLabels(int UserId)
        {
            try
            {
                return await labelRL.GetAllLabels(UserId);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<List<LabelResponseModel>> GetAllLabelsByLinqJoins(int UserId)
        {
           return await this.labelRL.GetAllLabelsByLinqJoins(UserId); 
        }


        public async Task<List<Label>> GetlabelByNotesId(int UserId,int NotesId)
        {
            try
            {
                return await labelRL.GetlabelByNotesId(UserId,NotesId);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

       public async Task <string> UpdateLabel(int UserId,int NoteId, string LableName)
        {
            try
            {
                return await labelRL.UpdateLabel(UserId,NoteId,LableName);
            }
            catch (Exception e)
            {

                throw e; 
            }
       }
        public async Task<string> DeleteLabel(int UserId,int NoteId)
        {
            try
            {
                return await labelRL.DeleteLabel(UserId, NoteId);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}
