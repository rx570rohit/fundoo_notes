using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DatabaseLayer.User
{
    public class NotePostModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Colour { get; set; }

       // [Required]
        //[RegularExpression("/^(0[1-9]|1\\|2\\|3[01])-(0[1-9]|1\\|2\\|3[01])-(19|20)\\{2}\\s+(0[0-9]|1[0-9]|2[0-3])\\:(0[0-9]|[1-5][0-9])$/")]
        //public DateTime Reminder { get; set; }
    }
}
