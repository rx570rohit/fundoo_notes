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
        Task AddNote(int userId, NotePostModel notePostModel);
        Task<List<Note>> GetAllNote(int userId);

        Task <List<Note>> GetNote(int NotesId ,int userId);

        Task<string> UpdateNote(int  userId ,NoteUpdateModel  noteUpdateModel, int NoteId);
        Task DeleteNotes(int userId,long NoteId);

        Task Reminder(int userId, int NoteId, DateTime dateTime);

        Task PinNote(int userId, int noteId);
        Task ArchiveNote(int userId, int noteId);

        Task ChangeNoteColour(int userId, int noteId,string colour);

    }
}
