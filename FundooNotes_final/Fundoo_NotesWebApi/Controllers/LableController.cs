using BuisnessLayer.Interface;
using DatabaseLayer.Lable;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Services;
using System;
using System.Linq;

namespace Fundoo_NotesWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LableController : ControllerBase
    {   
        ILabelBL labelBl;

        FundooContext fundooContext;

        public LableController(ILabelBL labelBl, FundooContext fundooContext)
        {
            this.labelBl = labelBl;
            this.fundooContext = fundooContext;
        }
        [Authorize]
        [HttpPost("user/AddLabel")]

      
        public IActionResult AddLabel(LabelModel labelModel)  
        {
            try
            {
                var currentUser = HttpContext.User;

                long userid = Convert.ToInt32(currentUser.Claims.FirstOrDefault(e => e.Type == "UserId").Value);
                var labelNote = this.fundooContext.Notes.Where(x => x.NoteId == labelModel.NoteId).SingleOrDefault();
                if (labelNote.UserId == userid)
                {
                    var result = this.labelBl.AddLabel(labelModel);
                    if (result)
                    {
                        return this.Ok(new { status = 200, isSuccess = true, Message = "Label created successfully!", data = result });
                    }
                    return this.BadRequest(new { status = 400, isSuccess = false, message = "Label not created" });
                }

                return this.BadRequest(new { status = 400, isSuccess = false, message = "Label not created" });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("GetAllLabels")]
        public IActionResult GetAllLabels()
        {
            try
            {
                var labels = this.labelBl.GetAllLabels();
                if (labels != null)
                {
                    return this.Ok(new { status = 200, isSuccess = true, Message = "lables are ready", data = labels });
                }
                else
                {
                    return this.NotFound(new { isSuccess = false, Message = "No label found" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Status = 401, isSuccess = false, message = e.InnerException.Message });
            }
        }

        [HttpGet("GetlabelByNotesId/{notesId}")]
        public IActionResult GetlabelByNotesId(int notesId)
        {
            try
            {
                var currentUser = HttpContext.User;

                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(X => X.Type == "UserId").Value);
                var labels = this.labelBl.GetlabelByNotesId(notesId);
                if (labels != null)
                {
                    return this.Ok(new { status = 200, isSuccess = true, message = "  label found Successfully", data = labels });
                }
                else
                {
                    return this.NotFound(new { isSuccess = false, message = "label not Found" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Status = 401, isSuccess = false, message = e.InnerException.Message });
            }
        }

        [HttpPut("UpdateLable")]
        public IActionResult UpdateLabel(LabelModel labelModel, int labelID)
        {
            try
            {
                var currentUser = HttpContext.User;

                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(X => X.Type == "UserId").Value);
                var result = this.labelBl.UpdateLabel(labelModel, labelID, userId);
                if (result != null)
                {
                    return this.Ok(new { status = 200, isSuccess = true, message = "Label Updated Successfully", data = result });
                }
                else
                {
                    return this.NotFound(new { isSuccess = false, message = "No Label Found" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Status = 401, isSuccess = false, message = e.InnerException.Message });
            }
        }


        [HttpDelete("DeleteLable")]
        public IActionResult DeleteLabel(int labelId,int NoteId)
        {
            try
            {
                var currentUser = HttpContext.User;

                int userid = Convert.ToInt32(currentUser.Claims.FirstOrDefault(X => X.Type == "UserId").Value);
                var delete = this.labelBl.DeleteLabel(labelId,NoteId);
                if (delete != null)
                {
                    return this.Ok(new { status = 200, isSuccess = true, message = "Label Deleted Successfully" });
                }
                else
                {
                    return this.NotFound(new { isSuccess = false, message = "Label not found" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Status = 401, isSuccess = false, message = e.InnerException.Message });
            }
        }
    }
}
