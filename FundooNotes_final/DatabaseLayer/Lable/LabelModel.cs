using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DatabaseLayer.Lable
{
    public class LabelModel
    {
        public string LabelName { get; set; }
        [Required]
        public int NoteId { get; set; }
    }
}
