using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Notes.Database;
using Notes.Domain.Entities;
using Notes.Domain.Interfaces;
using Notes.Domain.Models;

namespace Notes.Domain.Services
{
    public class NotesService : INotesService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly DatabaseContext _databaseContext;
        private readonly IIdenticonService _identiconService;

        public NotesService(UserManager<IdentityUser> userManager, DatabaseContext databaseContext,
            IIdenticonService identiconService)
        {
            _userManager = userManager;
            _databaseContext = databaseContext;
            _identiconService = identiconService;
        }

        public Note GetNote(int id)
        {
            return _databaseContext.Notes.First(x => x.Id.Equals(id));
        }

        public NoteViewModel GetNoteViewModel(int id)
        {
            var note = GetNote(id);
            return new NoteViewModel()
            {
                Id = note.Id, Identicon = note.Identicon, Text = note.Text, Title = note.Title
            };
        }

        public async Task<List<Note>> GetNotes(HttpContext context)
        {
            var user = await _userManager.GetUserAsync(context.User);
            var notes = new List<Note>();
            if (user != null)
            {
                notes = _databaseContext.Notes.Where(note => note.User.Id.Equals(user.Id)).ToList();
            }

            return notes;
        }

        public async Task CreateNoteAsync(NoteViewModel model, HttpContext context)
        {
            var note = new Note();
            var user = await _userManager.GetUserAsync(context.User);
            if (user != null)
            {
                note.Text = model.Text;
                note.Title = model.Title;
                note.User = user;
                note.Identicon =
                    _identiconService.GetIdenticon((model.Text + model.Title).GetHashCode().ToString());
                await _databaseContext.Notes.AddAsync(note);
                await _databaseContext.SaveChangesAsync();
            }
        }

        public async Task DeleteNoteAsync(int id)
        {
            var note = GetNote(id);
            if (note != null)
            {
                _databaseContext.Notes.Remove(note);
                await _databaseContext.SaveChangesAsync();
            }
        }

        public async Task UpdateNoteAsync(NoteViewModel model, int id)
        {
            var note = GetNote(id);
            note.Text = model.Text;
            note.Title = model.Title;
            note.Identicon =
                _identiconService.GetIdenticon((model.Text + model.Title).GetHashCode().ToString());
            _databaseContext.Update(note);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<List<Note>> FindNotesAsync(string searchString, HttpContext context)
        {
            var user = await _userManager.GetUserAsync(context.User);
            var notes = _databaseContext.Notes.Where(x =>
                (x.User.Id == user.Id) && (x.Text.ToLower().Contains(searchString.ToLower())
                                           || x.Title.ToLower().Contains(searchString.ToLower()))).ToList();
            return notes;
        }
    }
}