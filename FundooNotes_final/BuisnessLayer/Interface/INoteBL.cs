using DatabaseLayer.User;
using RepositoryLayer.Services.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface INoteBL
    {
        Task AddNote(int UserId, NotePostModel notePostModel);
        Task<List<Note>> GetAllNote(int userId);

        Task <List<Note>> GetNote(int NotesId);

        Task<string> UpdateNote(NotePostModel  noteUpdateModel, int NoteId);
        Task DeleteNotes(long NoteId);

    }
}
