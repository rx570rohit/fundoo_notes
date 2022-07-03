using BusinessLayer.Interfaces;
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

        public async Task<ActionResult> GetNote(int noteId)
        {
            try
            {
                var currentUser = HttpContext.User;
                var note = fundooContext.Notes.FirstOrDefault(u => u.NoteId == noteId);
                if (note == null)
                {

                    return this.BadRequest(new { success = true, message = "Note Doesn't Exits" });

                }
                List<Note> Note = new List<Note>();

                Note = await this.noteBL.GetNote(noteId);

                return Ok(new { success = true, message = "GetNote by id Successful", data = note });

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        [Authorize]
        [HttpPut("User/updateNotes")]

        public async Task<ActionResult> UpdateNotes(NotePostModel notePostModel ,int noteId)
        {
            try
            {
                var currentUser = HttpContext.User;
                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                await this.noteBL.UpdateNote(notePostModel , noteId);
                return this.Ok(new { success = true, message = "Note Updated Sucessfully" });
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        [Authorize]
        [HttpDelete("User/DeleteNotes")]

        public async Task<ActionResult> DeleteNotes(long noteId)
        {
            try
            {
                var currentUser = HttpContext.User;
                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);

                await this.noteBL.DeleteNotes(noteId);

                return Ok(new { success = true, message = "Note deleted Successfully"});
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
