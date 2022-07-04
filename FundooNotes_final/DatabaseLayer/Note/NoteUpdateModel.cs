using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DatabaseLayer.Note
{
    public class NoteUpdateModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Colour { get; set; }

        public bool IsArchive { get; set; }
        public bool IsPin { get; set; }
        public bool IsReminder { get; set; }
        public bool IsTrash { get; set; }
    }
}
