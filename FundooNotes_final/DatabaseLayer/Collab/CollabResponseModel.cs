using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseLayer.Collab
{
    public class CollabResponseModel
    {
        public int UserId { get; set; }

        public int NoteId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string CollabEmail { get; set; } 

       
        
    }
}
