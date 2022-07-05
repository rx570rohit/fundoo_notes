using DatabaseLayer.Lable;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Services
{
     public class LabelRL :ILabelRL
    {
        public readonly FundooContext fundooContext; //context class is used to query or save data to the database.
        public LabelRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }


        public bool AddLabel(LabelModel labelModel)
        {
            try
            {
                var note = fundooContext.Notes.Where(x => x.NoteId == labelModel.NoteId).FirstOrDefault();

                if (note != null)
                {
                    Label label = new Label();
                    label.LabelName = labelModel.LabelName;
                    label.NoteId = note.NoteId;
                    label.UserId = note.UserId;

                    this.fundooContext.Labels.Add(label);
                    int result = this.fundooContext.SaveChanges();
                    if (result > 0)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
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
                var result = this.fundooContext.Labels.ToList();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<Label> GetlabelByNotesId(int NotesId)
        {
            try
            {
                var response = this.fundooContext.Labels.Where(x => x.NoteId == NotesId).ToList();
                return response;
            }
            catch (Exception e)
            {
                throw e; ;
            }
        }

        public string UpdateLabel(LabelModel labelModel, int labelID ,int UserId)
        {
            try
            {
                var update = fundooContext.Labels.Where(X => X.LabelID == labelID && X.UserId == UserId).FirstOrDefault();
                if (update != null && update.LabelID == labelID)
                {
                    update.LabelName = labelModel.LabelName;
                    update.NoteId = labelModel.NoteId;

                    this.fundooContext.SaveChanges();
                    return "Label is modified";
                }
                else
                {
                    return "Label is not modified";
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public string DeleteLabel(int labelID ,int NoteId)
        {
            var deleteLabel = fundooContext.Labels.Where(X => X.LabelID == labelID && X.NoteId==NoteId).SingleOrDefault();
            if (deleteLabel != null)
            {
                fundooContext.Labels.Remove(deleteLabel);
                this.fundooContext.SaveChanges();
                return "Label Deleted Successfully";
            }
            else
            {
                return null;
            }
        }
    }
}
