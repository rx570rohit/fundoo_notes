using BusinessLayer.Interfaces;
using DatabaseLayer.Note;
using DatabaseLayer.User;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class NoteBL : INoteBL
    {
        INoteRL noteRL;

        public NoteBL(INoteRL noteRL)
        {
            this.noteRL = noteRL;
        }

        public async Task AddNote(int UserId, NotePostModel notePostModel)
        {
            try
            {
                await this.noteRL.AddNote(UserId, notePostModel);
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        public async Task<List<Note>> GetAllNote(int UserId)
        {
            try
            {
                return await this.noteRL.GetAllNote(UserId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        public async Task<List<Note>> GetNote(int UserId, int NotesId)
        {

            try
            {
                return await this.noteRL.GetNote(UserId,NotesId);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<string> UpdateNote(int UserId,NoteUpdateModel noteUpdateModel, int NoteId)
        {
            try
            {
                return await this.noteRL.UpdateNote(UserId,noteUpdateModel, NoteId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<string> DeleteNotes(int UserId,long NoteId)
        {
            try
            {
                 return await this.noteRL.DeleteNotes(UserId,NoteId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<string> Reminder(int UserId, int NoteId, DateTime dateTime)
        {
            try
            {
                return await this.noteRL.Reminder(UserId, NoteId, dateTime);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task ArchiveNote(int UserId, int noteId)
        {
            try
            {
                await this.noteRL.ArchiveNote(UserId, noteId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<string> PinNote(int UserId, int noteId)
        {
            try
            {
                return await this.noteRL.PinNote(UserId, noteId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task ChangeNoteColour(int userId, int noteId, string colour)
        {
            try
            {
                await this.noteRL.ChangeNoteColour(userId, noteId, colour);

            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<string> UnArchiveNote(int userId, int noteId)
        {
            try
            {
              return await this.noteRL.UnArchiveNote(userId,noteId);
            }
            catch (Exception e)
            {
                throw e;
            }       
        }

        public async Task<string> TrashNote(int userId,int noteId)
        {
            try
            {
               return await this.noteRL.TrashNote(userId, noteId);

            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}