using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseLayer.Label
{
    public class LabelResponseModel
    {

        public int UserId { get; set; }
        
        public int NoteId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string LabelName { get; set; }

    }
}
