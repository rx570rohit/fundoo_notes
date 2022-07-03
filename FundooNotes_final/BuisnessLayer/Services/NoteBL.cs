using BusinessLayer.Interfaces;
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

        public async Task<List<Note>> GetNote(int NotesId)
        {

            try
            {
                return await this.noteRL.GetNote(NotesId);
            }
            catch (Exception e)
            {

                throw e;
            }
        }


        public async Task<string> UpdateNote(NotePostModel noteUpdateModel, int NoteId)
        {
            try
            {
                return await this.noteRL.UpdateNote(noteUpdateModel, NoteId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task DeleteNotes(long NoteId)
        {
            try
            {
                 await this.noteRL.DeleteNotes(NoteId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}