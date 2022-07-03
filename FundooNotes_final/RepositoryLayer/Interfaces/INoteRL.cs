﻿using DatabaseLayer.User;
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
        Task <List<Note>> GetNote(int NotesId);
        Task<String> UpdateNote(NotePostModel noteUpdateModel, long noteId);
        Task DeleteNotes(long NoteId);
    }
}
