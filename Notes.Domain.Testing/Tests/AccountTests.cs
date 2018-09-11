using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Notes.Database;
using Notes.Domain.Interfaces;
using Notes.Domain.Models;
using NUnit.Framework;

namespace Notes.Domain.Testing.Tests
{
    [TestFixture]
    public class AccountTests
    {
        private IAccountService _accountService;
        private DatabaseContext _databaseContext;

        public AccountTests()
        {
            _accountService = Initializer.Provider.GetService<IAccountService>();
            _databaseContext = Initializer.Provider.GetService<DatabaseContext>();
        }

        
        private RegisterViewModel GetRegisterViewModel()
        {
            return new RegisterViewModel
            {
                UserName = Constants.FakeUserName,
                Email = Constants.FakeUserEmail,
                Password = Constants.FakeUserPassword
            };
        }
        private LoginViewModel GetLoginViewModel()
        {
            return new LoginViewModel()
            {
                Email = Constants.FakeUserEmail,
                Password = Constants.FakeUserPassword,
                RememberMe = false
            };
        }

        [TearDown]
        public async Task Dispose()
        {
            var users = _databaseContext.Users.ToList();
            _databaseContext.Users.RemoveRange(users);
            await _databaseContext.SaveChangesAsync();
        }

        [Test]
        public async Task RegisterAsync_WithCorrectParameters_ReturnsSuccessfulResult()
        {
            var registerResult = (await _accountService.RegisterAsync(GetRegisterViewModel())).Succeeded;
            var userResult = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Email == Constants.FakeUserEmail);
            Assert.IsTrue(registerResult);
            Assert.AreEqual(Constants.FakeUserName, userResult.UserName);
        }


        [Test]
        public async Task RegisterAsync_WithExistedUser_ReturnsFailedResult()
        {
            await _accountService.RegisterAsync(GetRegisterViewModel());
            var registerResult = (await _accountService.RegisterAsync(GetRegisterViewModel())).Succeeded;
            Assert.IsFalse(registerResult);
        }

        [Test]
        public async Task SignInAsync_WithNonExistentUser_ReturnsFailedResult()
        {
            var result = (await _accountService.SignInAsync(GetLoginViewModel())).Succeeded;
            Assert.IsFalse(result);
        }
    }
}