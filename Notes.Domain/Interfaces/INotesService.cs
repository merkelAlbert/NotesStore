using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Notes.Domain.Entities;
using Notes.Domain.Models;

namespace Notes.Domain.Interfaces
{
    public interface INotesService
    {
        Note GetNote(int id);
        NoteViewModel GetNoteViewModel(int id);
        Task<List<Note>> GetNotes(HttpContext context);
        Task CreateNoteAsync(NoteViewModel model, HttpContext context);
        Task DeleteNoteAsync(int id);
        Task UpdateNoteAsync(NoteViewModel model, int id);
        Task<List<Note>> FindNotesAsync(string searchString, HttpContext context);
        
    }
}

