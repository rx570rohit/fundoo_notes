using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuisnessLayer.Interface;
using DatabaseLayer.Collab;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RepositoryLayer.Services;
using RepositoryLayer.Services.Entities;

namespace Fundoo_NotesWebApi.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class CollabsController : ControllerBase
    {
        private readonly ICollabBL collabBL;
        private readonly FundooContext fundooContext;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;

        public CollabsController(ICollabBL collabBL, FundooContext fundooContext, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            this.collabBL = collabBL;
            this.fundooContext = fundooContext;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
        }

     
        [HttpPost("Add")]
        public async Task<IActionResult> AddCollab(CollabModel collabModel)
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserId").Value);
                var collab = this.fundooContext.Notes.Where(X => X.NoteId == collabModel.NoteId &&X.UserId==userId).FirstOrDefault();
                if (collab != null)
                {
                    if (collab.UserId == userId)
                    {
                        var result = await this.collabBL.AddCollab(userId, collabModel);
                        if (result != false)
                        {
                            return this.Ok(new { status = 200, isSuccess = true, message = "Collaboration established successfully" });
                        }
                        return this.BadRequest(new { status = 400, isSucess = false, message = "Failed to establish collaboration" });
                    }
                    else
                    {
                        return this.Unauthorized(new { status = 401, isSucess = false, Message = "Not authorized to add collaboration" });
                    }
                }
                else
                    return this.BadRequest(new { status = 400, success = false, message = "Invalid NoteId" });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Status = 401, isSuccess = false, Message = e.Message, InnerException = e.InnerException });
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllCollabs()
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserId").Value);
                var collabs = await collabBL.GetAllCollabs(userId);
                if (collabs != null)
                {
                    return this.Ok(new { isSuccess = true, message = " All Collaborators found Successfully", data = collabs });

                }
                else
                {
                    return this.NotFound(new { isSuccess = false, message = "No Collaborator  Found" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Status = 401, isSuccess = false, message = ex.InnerException.Message });
            }
        }

     
        [HttpGet("Redis")]
        public async Task<IActionResult> GetAllCollaboratorUsingRedisCache()
        {
            var cacheKey = "CollabsList";
            string serializedList;
            var CollabsList = new List<Collab>();
            var redisCollabsList = await distributedCache.GetAsync(cacheKey);
            if (redisCollabsList != null)
            {
                serializedList = Encoding.UTF8.GetString(redisCollabsList);
                CollabsList = JsonConvert.DeserializeObject<List<Collab>>(serializedList);
            }
            else
            {
                CollabsList = await fundooContext.collabs.ToListAsync();  // Comes from Microsoft.EntityFrameworkCore Namespace
                serializedList = JsonConvert.SerializeObject(CollabsList);
                redisCollabsList = Encoding.UTF8.GetBytes(serializedList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisCollabsList, options);
            }
            return Ok(CollabsList);
        }

     
        [HttpGet("Get/{noteId}")]
        public async Task<IActionResult> GetCollabsByNoteId(int noteId)
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserId").Value);
                var collabs = await collabBL.GetCollabsByNoteId(userId,noteId);
                if (collabs != null)
                {
                    return this.Ok(new { isSuccess = true, message = " All Collaborators found Successfully", data = collabs });

                }
                else
                {
                    return this.NotFound(new { isSuccess = false, message = "No Collaborator  Found" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Status = 401, isSuccess = false, message = ex.InnerException.Message });
            }
        }

        
        [HttpDelete("Remove/{noteId}")]
        public async Task<IActionResult> ReomoveCollab(int NoteId)
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(X => X.Type == "Id").Value);
                var delete = await this.collabBL.ReomoveCollab(userId,NoteId);
                if (delete != null)
                {
                    return this.Ok(new { status = 200, isSuccess = true, message = "Member removed from collaboration successfully" });
                }
                else
                {
                    return this.NotFound(new { isSuccess = false, message = "Member not removed from collaboration." });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Status = 401, isSuccess = false, Message = e.Message, InnerException = e.InnerException });
            }
        }
    }
}
