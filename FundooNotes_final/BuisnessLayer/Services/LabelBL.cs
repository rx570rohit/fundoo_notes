using BuisnessLayer.Interface;
using DatabaseLayer.Lable;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessLayer.Services
{
    public class LabelBL : ILabelBL
    {
        ILabelRL labelRL;
        public LabelBL(ILabelRL labelRL)
        {
            this.labelRL = labelRL;
        }

        public bool AddLabel(LabelModel labelModel)
        {
            try
            {
                return labelRL.AddLabel(labelModel);   
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public string DeleteLabel(int labelID, int NoteId)
        {
            try
            {
                return labelRL.DeleteLabel(labelID,NoteId);    
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public IEnumerable<Label> GetAllLabels()
        {
            try
            {
                return this.labelRL.GetAllLabels();
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public List<Label> GetlabelByNotesId(int NotesId)
        {
            try
            {
                return this.labelRL.GetlabelByNotesId(NotesId);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public string UpdateLabel(LabelModel labelModel, int labelID,int UserId)
        {
            try
            {
                return this.labelRL.UpdateLabel(labelModel, labelID,UserId);
            }
            catch (Exception e)
            {

                throw e; 
            }
        }
    }
}
