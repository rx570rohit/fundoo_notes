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
        public async Task DeleteNotes(int UserId,long NoteId)
        {
            try
            {
                 await this.noteRL.DeleteNotes(UserId,NoteId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task Reminder(int UserId, int NoteId, DateTime dateTime)
        {
            try
            {
                await this.noteRL.Reminder(UserId, NoteId, dateTime);
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
        public async Task PinNote(int UserId, int noteId)
        {
            try
            {
                await this.noteRL.PinNote(UserId, noteId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}