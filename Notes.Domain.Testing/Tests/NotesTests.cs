using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Notes.Database;
using Notes.Domain.Entities;
using Notes.Domain.Interfaces;
using Notes.Domain.Models;
using Notes.Domain.Testing.Wrappers;
using NUnit.Framework;

namespace Notes.Domain.Testing.Tests
{
    [TestFixture]
    public class NotesTests
    {
        private readonly INotesService _notesService;
        private readonly DatabaseContext _databaseContext;
        private readonly IIdenticonService _identiconService;
        private readonly NotesWrapper _notesWrapper;

        private const string Title = "Заметка";
        private const string Text = "Текст заметки";
        private const int NotesAmount = 10;

        private NoteViewModel GetNoteViewModel(string title, string text)
        {
            var note = new NoteViewModel();
            note.Identicon = _identiconService.GetIdenticon("");
            note.Text = text;
            note.Title = title;
            return note;
        }

        public NotesTests()
        {
            _notesService = Initializer.Provider.GetService<INotesService>();
            _databaseContext = Initializer.Provider.GetService<DatabaseContext>();
            _identiconService = Initializer.Provider.GetService<IIdenticonService>();
            _notesWrapper = Initializer.Provider.GetService<NotesWrapper>();
        }

        [TearDown]
        public async Task Dispose()
        {
            var notes = _databaseContext.Notes.ToList();
            _databaseContext.Notes.RemoveRange(notes);
            await _databaseContext.SaveChangesAsync();
        }

        [Test]
        public async Task CreateNoteAsync_WithCorrectParameters_CreatesNote()
        {
            await _notesService.CreateNoteAsync(GetNoteViewModel(Title, Text), Constants.UserId);
            var result = await _databaseContext.Notes.FirstAsync(x => x.Title == Title);
            Assert.AreEqual(Text, result.Text);
        }

        [Test]
        public async Task GetNoteAsync_WithNonExistentId_ReturnsNull()
        {
            var result = await _notesService.GetNoteAsync(1);
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetNotesAsync_WithUserId_ReturnsUserNotesList()
        {
            for (int i = 0; i < NotesAmount; i++)
            {
                await _notesWrapper.CreateNoteWithFakeUserAsync(Title, Text);
            }

            var result = _notesService.GetNotes(Constants.UserId).Count;
            Assert.AreEqual(NotesAmount, result);
        }

        [Test]
        public async Task DeleteNoteAsync_WithCorrectId_DeletesNote()
        {
            await _notesService.CreateNoteAsync(GetNoteViewModel(Title, Text), Constants.UserId);
            var id = (await _databaseContext.Notes.FirstAsync(x => x.Title == Title)).Id;
            await _notesService.DeleteNoteAsync(id);
            var result = await _databaseContext.Notes.FirstOrDefaultAsync(x => x.Id == id);
            Assert.IsNull(result);
        }

        [Test]
        public async Task UpdateNoteAsync_WithNewData_UpdatesNote()
        {
            await _notesService.CreateNoteAsync(GetNoteViewModel(Title, Text), Constants.UserId);
            var id = (await _databaseContext.Notes.FirstAsync(x => x.Title == Title)).Id;
            var newTitle = Title + "1";
            var newText = Text + "1";
            await _notesService.UpdateNoteAsync(GetNoteViewModel(newTitle, newText), id);
            var note = await _databaseContext.Notes.FirstOrDefaultAsync(x => x.Id == id);
            bool result = note.Title == newTitle && note.Text == newText;
            Assert.IsTrue(result);
        }

        [Test]
        public async Task FindNotesAsync_WithSearchStringByText_ReturnsFindedNotes()
        {
            for (int i = 0; i < NotesAmount; i++)
            {
                await _notesWrapper.CreateNoteWithFakeUserAsync(Title + i, Text + i);
            }

            var result = (await _notesService.FindNotesAsync(Text, Constants.UserId)).Count;
            Assert.AreEqual(NotesAmount, result);
        }

        [Test]
        public async Task FindNotesAsync_WithSearchStringByNonExistentValue_ReturnsEmptyList()
        {
            for (int i = 0; i < NotesAmount; i++)
            {
                await _notesWrapper.CreateNoteWithFakeUserAsync(Title + i, Text + i);
            }

            var result = (await _notesService.FindNotesAsync("test", Constants.UserId));
            Assert.IsEmpty(result);
        }
    }
}