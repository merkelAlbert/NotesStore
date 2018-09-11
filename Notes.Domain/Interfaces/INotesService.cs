using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Notes.Domain.Entities;
using Notes.Domain.Models;

namespace Notes.Domain.Interfaces
{
    public interface INotesService
    {
        Task<Note> GetNoteAsync(int id);
        NoteViewModel GetNoteViewModel(int id);
        List<Note> GetNotes(string userId);
        Task CreateNoteAsync(NoteViewModel model, string userId);
        Task DeleteNoteAsync(int id);
        Task UpdateNoteAsync(NoteViewModel model, int id);
        Task<List<Note>> FindNotesAsync(string searchString, string userId);
        
    }
}

