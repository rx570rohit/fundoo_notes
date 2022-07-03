using DatabaseLayer.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class NoteRL : INoteRL
    {
        FundooContext fundooContext;

        IConfiguration configuration;

        //private readonly string _secret;

        public NoteRL(FundooContext fundooContext, IConfiguration configuration)
        {
            this.fundooContext = fundooContext;
            this.configuration = configuration;
        }



        public async Task AddNote(int UserId, NotePostModel notePostModel)
        {
            try
            {
                Note note = new Note();
                note.UserId = UserId;
                note.Title = notePostModel.Title;
                note.Description = notePostModel.Description;
                note.Colour = notePostModel.Colour;
                note.Reminder = DateTime.Now.AddDays(7);   
                note.CreatedDate = DateTime.Now;
                note.ModifiedDate = DateTime.Now;
                fundooContext.Add(note);

                await fundooContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<List<Note>> GetAllNote(int userId)
        {
            try
            {
                var note = fundooContext.Notes.Where(u => u.UserId == userId).FirstOrDefault();
                if (note == null)
                {
                    return null;
                }
                return await fundooContext.Notes.ToListAsync();
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
                var listNote = fundooContext.Notes.Where(X => X.NoteId == NotesId).SingleOrDefault();
                if (listNote != null)
                {
                    return await fundooContext.Notes.Where(list => list.NoteId == NotesId).ToListAsync();
                }
                return null;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        

       

         public async Task<string> UpdateNote(NotePostModel noteUpdateModel, long noteId)
        {
            try
            {
                var update = fundooContext.Notes.Where(X => X.NoteId == noteId).FirstOrDefault();
                if (update != null && update.NoteId == noteId)
                {
                    update.Title = noteUpdateModel.Title;
                    update.Description = noteUpdateModel.Description;
                    update.ModifiedDate = DateTime.Now;
                    update.Colour = noteUpdateModel.Colour;

                    this.fundooContext.SaveChanges();
                    return  "Note is Modified";
                }
                else
                {
                    return "Note Not Modified";
                }
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
                var deleteNote = fundooContext.Notes.Where(X => X.NoteId == NoteId).SingleOrDefault();
                if (deleteNote != null)

                    fundooContext.Notes.Remove(deleteNote);
                this.fundooContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }
    }
}
