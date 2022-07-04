﻿using DatabaseLayer.Note;
using DatabaseLayer.User;
using RepositoryLayer.Services.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface INoteRL
    {
        Task AddNote(int UserId, NotePostModel notePostModel);
        Task<List<Note>> GetAllNote(int userId);
        Task <List<Note>> GetNote(int UserId,int NotesId);
        Task<String> UpdateNote(int UserId,NoteUpdateModel noteUpdateModel, long noteId);
        Task DeleteNotes(int UserId,long NoteId);

        Task Reminder(int UserId, int NoteId, DateTime dateTime);
        Task PinNote(int UserId, int noteId);

        Task ArchiveNote(int UserId, int noteId);
        Task ChangeNoteColour(int userId, int noteId, String Colour);


    }
}
