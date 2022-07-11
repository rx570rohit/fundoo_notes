using DatabaseLayer.Collab;
using RepositoryLayer.Services.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface ICollabRL
    {

        Task<bool> AddCollab(int userId, CollabModel collabModel);
        Task<IEnumerable<CollabResponseModel>> GetAllCollabs(int userId);
        Task<IEnumerable<CollabResponseModel>> GetCollabsByNoteId(int userId, int noteId);
        Task<string> ReomoveCollab(int UserId, int noteId);
    }
}
