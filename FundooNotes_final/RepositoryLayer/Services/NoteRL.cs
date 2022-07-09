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

        public async Task<List<Note>> GetAllNote()
        {
            try
            {
                var note = fundooContext.Notes.FirstOrDefault();
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
                var update = fundooContext.Notes.Where(X => X.NoteId == noteId && X.UserId == UserId).FirstOrDefault();
                if (update != null && update.NoteId == noteId)
                {
                    update.Title = noteUpdateModel.Title;
                    update.Description = noteUpdateModel.Description;
                    update.ModifiedDate = DateTime.Now;
                    update.Colour = noteUpdateModel.Colour;

                    await this.fundooContext.SaveChangesAsync();
                    return "Note is Modified successfully";
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> DeleteNotes(int UserId, long NoteId)
        {
            try
            {
                var deleteNote = fundooContext.Notes.Where(X => X.NoteId == NoteId && X.UserId == UserId).SingleOrDefault();
                if (deleteNote != null)
                {
                    fundooContext.Notes.Remove(deleteNote);
                    await this.fundooContext.SaveChangesAsync();
                    return "Note is Deleted successfully";
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public async Task<string> Reminder(int userId, int noteId, DateTime dateTime)
        {
            try
            {
                var reminder = fundooContext.Notes.Where(x => x.NoteId == noteId && x.UserId == userId).FirstOrDefault();
                if (reminder != null)
                {
                    reminder.Reminder = dateTime;
                    await this.fundooContext.SaveChangesAsync();
                    return "Reminder Set Successfull for date:" + dateTime.Date+" And Time : "+dateTime.TimeOfDay;
                }
                else 
                {
                    return null;
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }


        public async Task<string> PinNote(int userId, int noteId)
        {
            try
            {
                var pined= this.fundooContext.Notes.Where(p => p.NoteId == noteId && p.UserId == userId).FirstOrDefault();
                if (pined.IsPin == true)
                {
                    pined.IsPin= false;
                    await this.fundooContext.SaveChangesAsync();
                    return "note PinnedNote Successfully";
                }
                else
                {
                    pined.IsPin = true;
                    await this.fundooContext.SaveChangesAsync();
                    return "note UnPinned Successfully";
                }

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

        public async Task ChangeNoteColour(int userId, int noteId, string colour)
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

        public async Task<string> UnArchiveNote(int userId, int noteId)
        {

            try
            {
                var response = this.fundooContext.Notes.Where(a => a.NoteId == noteId && a.IsArchive == true && a.UserId==userId).SingleOrDefault();
                if (response != null)
                {
                    response.IsArchive = false;
                    await this.fundooContext.SaveChangesAsync();
                    return "Note is UnArchived successfully";
                }
                else if (this.fundooContext.Notes.Where(a => a.NoteId == noteId && a.IsArchive == false && a.UserId == userId).SingleOrDefault()!=null)
                {
                    return "Note is already UnArchived";
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<string> TrashNote(int userId, int noteId)
        {
            try
            { 
               var trashed = this.fundooContext.Notes.Where(p => p.NoteId == noteId && p.UserId==userId).FirstOrDefault();
                  if (trashed.IsTrash == true)
                  {
                        trashed.IsTrash = false;
                        await this.fundooContext.SaveChangesAsync();
                        return "notes recoverd";
                  }
                  else
                  {
                        trashed.IsTrash = true;
                        await this.fundooContext.SaveChangesAsync();
                        return "note is in trashed";
                  }
                
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}


