using DatabaseLayer.Note;
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

        Task <List<Note>> GetNote(int NotesId ,int UserId);

        Task<string> UpdateNote(int  UserId ,NoteUpdateModel  noteUpdateModel, int NoteId);
        Task DeleteNotes(int UserId,long NoteId);

        Task Reminder(int UserId, int NoteId, DateTime dateTime);

        Task PinNote(int UserId, int noteId);
        Task ArchiveNote(int UserId, int noteId);

    }
}
