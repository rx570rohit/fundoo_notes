using BuisnessLayer.Interface;
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

        [HttpGet("GetAllLabels")]
        public async Task<IActionResult> GetAllLabels()
        {
            try
            {
                var currentUser = HttpContext.User;

                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(e => e.Type == "UserId").Value);
                var labels = await labelBl.GetAllLabels(userId);
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
        [Authorize]
        [HttpGet("GetAllLabelsUsingRedisCache")]
        public async Task<IActionResult> GetAllLabelUsingRedisCache()
        {
           // cacheKey = "LabelsList";

            string serializedLabelsList;
            var currentUser = HttpContext.User;
            int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(e => e.Type == "UserId").Value);
            var labelsList = await labelBl.GetAllLabels(userId);

            var redisLabelsList = await this.distributedCache.GetAsync(cacheKey);

            if (labelsList != null)
            {

                if (redisLabelsList != null)
                {
                    serializedLabelsList = Encoding.UTF8.GetString(redisLabelsList);
                    labelsList = JsonConvert.DeserializeObject<List<Label>>(serializedLabelsList);
                }
                else
                {
                    labelsList = await this.fundooContext.Label.ToListAsync();  // Comes from Microsoft.EntityFrameworkCore Namespace
                    serializedLabelsList = JsonConvert.SerializeObject(labelsList);
                    redisLabelsList = Encoding.UTF8.GetBytes(serializedLabelsList);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                    await this.distributedCache.SetAsync(cacheKey, redisLabelsList, options);
                }
                var labels = labelsList;
                return this.Ok(new { status = 200, isSuccess = true, message = "All lables are loaded", data = labels });
            }
            else
            {
                return this.BadRequest(new { Status = 401, isSuccess = false, message = "labelList Not found " });
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
                    return this.NotFound(new { isSuccess = false, message = "label not Found" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Status = 401, isSuccess = false, message = e.InnerException.Message });
            }
        }

        [HttpPut("{NoteId}/{LabelName}")]
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
