using DatabaseLayer.Note;
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

        public async Task<List<Note>> GetNote(int UserId, int NotesId)
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


        public async Task<string> UpdateNote(int UserId, NoteUpdateModel noteUpdateModel, long noteId)
        {
            try
            {
                var update = fundooContext.Notes.Where(X => X.NoteId == noteId && X.UserId==UserId).FirstOrDefault();
                if (update != null && update.NoteId == noteId)
                {
                    update.Title = noteUpdateModel.Title;
                    update.Description = noteUpdateModel.Description;
                    update.ModifiedDate = DateTime.Now;
                    update.Colour = noteUpdateModel.Colour;

                 await this.fundooContext.SaveChangesAsync();
                    return "Note is Modified";
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

        public async Task DeleteNotes(int UserId, long NoteId)
        {
            try
            {
                var deleteNote = fundooContext.Notes.Where(X => X.NoteId == NoteId && X.UserId == UserId).SingleOrDefault();
                if (deleteNote != null)

                    fundooContext.Notes.Remove(deleteNote);
               await this.fundooContext.SaveChangesAsync();
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
                var reminder = fundooContext.Notes.Where(x => x.NoteId == NoteId && x.UserId == UserId).FirstOrDefault();

                reminder.Reminder = dateTime;

              await this.fundooContext.SaveChangesAsync();
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
                var note = fundooContext.Notes.Where(x => x.UserId == UserId && x.NoteId == noteId).FirstOrDefault();
                if (note != null)
                {
                    if (note.IsTrash == false)
                    {
                        if (note.IsPin == false)
                        {
                            note.IsPin = true;
                        }
                        else
                        {
                            note.IsPin = false;
                        }
                    }
                }
                await fundooContext.SaveChangesAsync();
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
                var note = fundooContext.Notes.Where(x => x.UserId == UserId && x.NoteId == noteId).FirstOrDefault();
                if (note != null)
                {
                    if (note.IsTrash == false)
                    {
                        if (note.IsArchive == false)
                        {
                            note.IsArchive = true;
                        }
                        else
                        {
                            note.IsArchive = false;
                        }
                    }
                }
                await fundooContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task ChangeNoteColour(int userId, int noteId,string colour)
        {
            try
            {
                var change = fundooContext.Notes.Where(X => X.NoteId == noteId && X.UserId == userId).FirstOrDefault();
                if (change != null && change.NoteId == noteId)
                {

                    change.Colour = colour;

                    await this.fundooContext.SaveChangesAsync();
                }
            }

            catch (Exception e)
            {

                throw e;
            }
        }
    }
}

