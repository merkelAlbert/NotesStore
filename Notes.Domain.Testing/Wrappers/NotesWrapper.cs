using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Notes.Database;
using Notes.Domain.Entities;
using Notes.Domain.Interfaces;

namespace Notes.Domain.Testing.Wrappers
{
    public class NotesWrapper
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IIdenticonService _identiconService;


        public NotesWrapper(DatabaseContext databaseContext, IIdenticonService identiconService)
        {
            _databaseContext = databaseContext;
            _identiconService = identiconService;
        }

        public async Task CreateNoteWithFakeUserAsync(string title, string text)
        {
            var note = new Note
            {
                User = new IdentityUser
                {
                    Id = Constants.UserId,
                    UserName = Constants.FakeUserName,
                    Email = Constants.FakeUserEmail,
                    PasswordHash = Constants.FakeUserPassword
                },
                Identicon = _identiconService.GetIdenticon(""),
                Text = text,
                Title = title
            };
            _databaseContext.Notes.Add(note);
            await _databaseContext.SaveChangesAsync();
        }
    }
}