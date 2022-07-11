using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RepositoryLayer.Services.Entities
{
    [Keyless]
    public class Label
    {
       
    //    public int LabelID { get; set; }
        public string LabelName { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }

        [ForeignKey("Notes")]
        public int NoteId { get; set; }
    }
}
