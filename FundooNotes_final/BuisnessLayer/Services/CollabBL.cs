using BuisnessLayer.Interface;
using DatabaseLayer.Collab;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer.Services
{
    
    public class CollabBL : ICollabBL
    {
        ICollabRL collabRL;
        public CollabBL(ICollabRL collabRL)
        {
            this.collabRL = collabRL;
        }
        public async Task<bool> AddCollab(int userId,CollabModel collabModel)
        {
            try
            {
                return await collabRL.AddCollab(userId ,collabModel);
            }
            catch (Exception e)
            {

                throw e;
            }
          
        }

        public async Task<IEnumerable<CollabResponseModel>> GetAllCollabs(int userId)
        {
            try
            {
             return await collabRL.GetAllCollabs(userId);   
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<CollabResponseModel>> GetCollabsByNoteId(int userId, int noteId)
        {

            try
            {
                return await collabRL.GetCollabsByNoteId(userId, noteId);
            }
            catch (Exception e)
            {

                throw e;
            }        
        }

        public async Task<string> ReomoveCollab(int UserId, int noteId)
        {

            try
            {
               return await collabRL.ReomoveCollab(UserId, noteId); 
            }
            catch (Exception e)
            {

                throw e;
            }       
        }
    }
}
