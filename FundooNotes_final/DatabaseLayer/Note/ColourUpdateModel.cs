using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace DatabaseLayer.Note
{
    public class ColourUpdateModel
    {
        [Required]
      
        public string Colour { get; set; }
    }
}
