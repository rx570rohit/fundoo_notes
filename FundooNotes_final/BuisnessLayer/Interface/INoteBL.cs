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
        Task<List<Note>> GetAllNote();

        Task <List<Note>> GetNote(int NotesId ,int userId);

        Task<string> UpdateNote(int  userId ,NoteUpdateModel  noteUpdateModel, int NoteId);
        Task<string> DeleteNotes(int userId,long NoteId);

        Task<string> Reminder(int userId, int NoteId, DateTime dateTime);

        Task<string> PinNote(int userId, int noteId);
        Task ArchiveNote(int userId, int noteId);

        Task ChangeNoteColour(int userId, int noteId,string colour);

        Task<string> UnArchiveNote(int userId,int noteId);

        Task<string> TrashNote(int userId,int notesId);

    }
}
