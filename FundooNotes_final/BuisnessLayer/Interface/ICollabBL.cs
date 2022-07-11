using DatabaseLayer.Collab;
using RepositoryLayer.Services.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer.Interface
{
    public interface ICollabBL
    {
        Task<bool> AddCollab(int userId, CollabModel collabModel);
        Task<IEnumerable<CollabResponseModel>> GetAllCollabs(int userId);
        Task<IEnumerable<CollabResponseModel>> GetCollabsByNoteId(int userId, int noteId);
        Task<string> ReomoveCollab(int UserId, int noteId);
    }
}
