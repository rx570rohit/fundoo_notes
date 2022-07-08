using BuisnessLayer.Interface;
using DatabaseLayer.Label;
using DatabaseLayer.Lable;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RepositoryLayer.Services;
using RepositoryLayer.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundoo_NotesWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LableController : ControllerBase
    {   
        ILabelBL labelBl;

        FundooContext fundooContext;
        private readonly IDistributedCache distributedCache;
        IConfiguration Configuration;
        private string cacheKey;

        public LableController(ILabelBL labelBl, FundooContext fundooContext, IDistributedCache distributedCache , IConfiguration configuration)
        {
            this.labelBl = labelBl;
            this.fundooContext = fundooContext;
            this.distributedCache = distributedCache; 
            this.Configuration = configuration;
            cacheKey = configuration.GetSection("redis").GetSection("cacheKey").Value;

        }
        [Authorize]
        [HttpPost("AddLabel/{NoteId}/{LabelName}")]

      
        public async Task<IActionResult> AddLabel(int NoteId,string LabelName)  
        {
            try
            {
                var currentUser = HttpContext.User;

                int userid = Convert.ToInt32(currentUser.Claims.FirstOrDefault(e => e.Type == "UserId").Value);
                var Label = fundooContext.Label.Where(x => x.UserId == userid && x.NoteId == NoteId).FirstOrDefault();

                if (Label != null)
                {
                    return this.BadRequest(new { status = 301, isSuccess = false, Message = "Enter Distinct NoteId" });

                }
                await this.labelBl.AddLabel(userid, NoteId, LabelName);

                return this.Ok(new { status = 200, isSuccess = true, Message = "Label created successfully!" });

            }   
            catch (Exception e)
            {
                throw e;
            }
        }

       
        [Authorize]
        [HttpGet("GetAllLabelsUsingJoins")]

        public async Task<IActionResult> GetAllLabelsByLinqJoins()
        {
            try
            {
                var currentUser = HttpContext.User;
                var userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var labels = await labelBl.GetAllLabelsByLinqJoins(userId);
                if (labels != null)
                {
                    return this.Ok(new { status = 200, success = true, message = "All Label are ready", data = labels });
                }
                else { return this.NotFound(new { success = false, message = "No Label found" }); }
            }
            catch (Exception e)
            {
                throw e;
            }
         }

      


        [HttpGet("GetlabelsByNotesId/{notesId}")]

       public async Task<IActionResult> GetlabelByNotesId(int notesId)
        {
            try
            {
                var currentUser = HttpContext.User;

                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(X => X.Type == "UserId").Value);
                var labels = await labelBl.GetlabelByNotesId(userId,notesId);
                if (labels.Count != 0)
                {
                    return this.Ok(new { status = 200, isSuccess = true, message = "  label found Successfully", data = labels });
                }
                else
                {
                    return this.NotFound(new { isSuccess = false, message = "Label not Found" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Status = 401, isSuccess = false, message = e.InnerException.Message });
            }
        }

        [HttpPut("UpdateLabel/{NoteId}/{LabelName}")]
        public async Task<IActionResult> UpdateLabel(int NoteId,string LabelName)
        {
            try
            {
                var currentUser = HttpContext.User;

                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(X => X.Type == "UserId").Value);

                var result = await labelBl.UpdateLabel(userId,NoteId,LabelName);
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


        [HttpDelete("{NoteId}")]
        public async Task<IActionResult> DeleteLabel(int NoteId)
        {
            try
            {
                var currentUser = HttpContext.User;

                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(X => X.Type == "UserId").Value);
                var delete = await labelBl.DeleteLabel(userId,NoteId);
                if (delete != null)
                {
                    return this.Ok(new
                    {
                        status = 200,
                        isSuccess = true,
                        message = "Label Deleted Successfully"
                    });
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
