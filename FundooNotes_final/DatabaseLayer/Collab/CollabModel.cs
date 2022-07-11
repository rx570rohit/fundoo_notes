using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DatabaseLayer.Collab
{ 
    public class CollabModel
    {
        [Required]
        [RegularExpression("^[a-z A-Z 0-9]+([._#%+][a-z A-Z 0-9]+)?[@][a-z A-z]+[.][a-z A-Z]{2,3}([.][a-z]{2})?$", ErrorMessage = "Enter a Valid Email-Id")]
        public string CollabEmail { get; set; }

        public long NoteId { get; set; }
    }
}
