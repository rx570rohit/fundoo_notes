﻿using DatabaseLayer.Lable;
using RepositoryLayer.Services.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface ILabelRL
    {
        public bool AddLabel(LabelModel labelModel);
        public IEnumerable<Label> GetAllLabels();
        public List<Label> GetlabelByNotesId(int NotesId);
        public string UpdateLabel(LabelModel labelModel, int labelID,int UserId);
        public string DeleteLabel(int labelID,int NoteId);
    }
}
