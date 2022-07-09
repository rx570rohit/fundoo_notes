using DatabaseLayer.Label;
using DatabaseLayer.Lable;
using RepositoryLayer.Services.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface ILabelRL
    {
        Task AddLabel(int UserId,int NoteId,string labelName);
        Task <List<LabelResponseModel>> GetlabelByNotesId(int UserId,int NotesId);
        Task <string> UpdateLabel(int UserId,int NoteId,string LabelName);
        Task<string> DeleteLabel(int UserId,int NoteId);
        Task<List<LabelResponseModel>> GetAllLabelsByLinqJoins(int UserId);
    }
}
