using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RepositoryLayer.Services.Entities
{
    [Keyless]
    public class Collab
    {
        public string CollabEmail { get; set; }

        [ForeignKey("Users")]
        public long UserId { get; set; }

        [ForeignKey("Notes")]
        public long NoteId { get; set; }

    }
}
