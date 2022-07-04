using BusinessLayer.Interfaces;
using DatabaseLayer.Note;
using DatabaseLayer.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Services;
using RepositoryLayer.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundooNote.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NoteController : ControllerBase
    {
        INoteBL noteBL;
        FundooContext fundooContext;

        public NoteController(INoteBL noteBL, FundooContext fundooContext)
        {
            this.noteBL = noteBL;
            this.fundooContext = fundooContext;
        }

        [Authorize]
        [HttpPost("user/AddNotes")]
        public async Task<ActionResult> AddNote(NotePostModel notePostModel)
        {
            try
            {
                var currentUser = HttpContext.User;
                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                await this.noteBL.AddNote(userId, notePostModel);
                return this.Ok(new { success = true, message = "Note Added Sucessfully" });
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        [Authorize]
        [HttpGet("User/GetAll")]

        public async Task<ActionResult> GetAllNote()
        {
            try
            {
                var currentUser = HttpContext.User;
                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                var note = fundooContext.Notes.FirstOrDefault(u => u.UserId == userId);
                if (note == null)
                {

                    return this.BadRequest(new { success = true, message = "Note Doesn't Exits" });

                }
                List<Note> noteList = new List<Note>();

                noteList = await this.noteBL.GetAllNote(userId);

                return Ok(new { success = true, message = "GetAllNote Successfully", data = noteList });

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        [Authorize]
        [HttpGet("User/getNote")]

        public async Task<ActionResult> GetNote( int noteId)
        {
            try
            {
                var currentUser = HttpContext.User;
                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                var note = fundooContext.Notes.FirstOrDefault(u => u.NoteId == noteId);
                if (note == null)
                {

                    return this.BadRequest(new { success = true, message = "Note Doesn't Exits" });

                }
                List<Note> Note = new List<Note>();

                Note = await this.noteBL.GetNote(userId,noteId);

                return Ok(new { success = true, message = "GetNote by id Successful", data = note });

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        [Authorize]
        [HttpPut("User/updateNotes")]

        public async Task<ActionResult> UpdateNotes( NoteUpdateModel noteUpdateModel ,int noteId)
        {
            try
            {
                var currentUser = HttpContext.User;
                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                await this.noteBL.UpdateNote(userId,noteUpdateModel, noteId);
                return this.Ok(new { success = true, message = "Note Updated Sucessfully" });
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        [Authorize]
        [HttpDelete("User/DeleteNotes{NoteId}")]

        public async Task<ActionResult> DeleteNotes( long NoteId)
        {
            try
            {
                var currentUser = HttpContext.User;
                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);

                await this.noteBL.DeleteNotes(userId,NoteId);

                return Ok(new { success = true, message = "Note deleted Successfully"});
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Authorize]
        [HttpPut("User/Reminder/{NoteId}")]

        public async Task<IActionResult> Reminder(int NoteId,ReminderUpdateModel reminderUpdateModel)
        {
            try
            {
                var currentUser = HttpContext.User;
                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                await this.noteBL.Reminder(userId, NoteId, Convert.ToDateTime( reminderUpdateModel.Reminder));
                return Ok(new { success = true, message = "Reminder set Successfully" });

            }
            catch (Exception e)
            {

                throw e;
            }

        }
        [Authorize]
        [HttpPut("ArchiveNote/{NoteId}")]

        public async Task<ActionResult> ArchiveNote(int NoteId)
        {
            try
            {
                var currentUser = HttpContext.User;
                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                var note = fundooContext.Notes.FirstOrDefault(u => u.UserId == userId && u.NoteId == NoteId);
                if (note == null)
                {

                    return this.BadRequest(new { success = true, message = "Sorry! Note Doesn't Exist Please Create a Notes" });

                }
                await this.noteBL.ArchiveNote(userId, NoteId);

                return Ok(new { success = true, message = $"Note Archive Successfully for the note, {note.Title} " });

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Authorize]
        [HttpPut("PinNote/{NoteId}")]

        public async Task<ActionResult> PinNote(int NoteId)
        {
            try
            {
                var currentUser = HttpContext.User;
                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                var note = fundooContext.Notes.FirstOrDefault(u => u.UserId == userId && u.NoteId == NoteId);
                if (note == null)
                {

                    return this.BadRequest(new { success = true, message = "Sorry! Note Doesn't Exist Please Create a Notes" });

                }
                await this.noteBL.PinNote(userId, NoteId);

                return Ok(new { success = true, message = $"Note Pinned Successfully for the note, {note.Title} " });

            }
            catch (Exception e)
            {
                throw e;
            }

        }
        [Authorize]
        [HttpPut("ChangeNoteColour/{NoteId}")]

        public async Task<ActionResult> ChangeNoteColour(int noteId, ColourUpdateModel colourUpdateModel)
        {
            

            try
            {
                var currentUser = HttpContext.User;
                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                var note = fundooContext.Notes.FirstOrDefault(u => u.UserId == userId && u.NoteId == noteId);

                if (note == null)
                {

                    return this.BadRequest(new { success = true, message = "Sorry! Note Doesn't Exist Please Create a Notes" });

                }
                await this.noteBL.ChangeNoteColour(userId,noteId, colourUpdateModel.Colour);

                return Ok(new { success = true, message = $"Note Pinned Successfully for the note, {note.Title} " });

            }
            catch (Exception e)
            {

                throw e;
            }
           

        }


    }
}
