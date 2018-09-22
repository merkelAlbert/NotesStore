using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Notes.Database;
using Notes.Domain.Enums;
using Notes.Domain.Interfaces;
using Notes.Domain.Utils;
using NUnit.Framework;
using OfficeOpenXml;

namespace Notes.Domain.Testing.Tests
{
    [TestFixture]
    public class AdminTests
    {
        private readonly DatabaseContext _databaseContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly RolesInitializer _rolesInitializer;
        private readonly IAdminService _adminService;

        private const string FileName = "test";

        public AdminTests()
        {
            _adminService = Initializer.Provider.GetService<IAdminService>();
            _databaseContext = Initializer.Provider.GetService<DatabaseContext>();
            _rolesInitializer = Initializer.Provider.GetService<RolesInitializer>();
            _userManager = Initializer.Provider.GetService<UserManager<IdentityUser>>();
        }

        private IdentityUser GetUser()
        {
            return new IdentityUser
            {
                Email = Constants.FakeUserEmail,
                UserName = Constants.FakeUserName
            };
        }

        [SetUp]
        public async Task InitRoles()
        {
            await _rolesInitializer.InitializeAsync();
        }

        [TearDown]
        public async Task Dispose()
        {
            File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName));
            var users = _databaseContext.Users.ToList();
            var notes = _databaseContext.Notes.ToList();
            _databaseContext.Users.RemoveRange(users);
            _databaseContext.Notes.RemoveRange(notes);
            await _databaseContext.SaveChangesAsync();
        }

        [Test]
        public async Task ChangeUserRole_FromUserToAdmin_ChangesRoleToAdmin()
        {
            await _userManager.CreateAsync(GetUser(), Constants.FakeUserPassword);
            var user = await _userManager.FindByEmailAsync(Constants.FakeUserEmail);
            await _adminService.ChangeUserRoleAsync(user.Id.ToString(), Role.Admin.ToString());
            var result = (await _userManager.GetRolesAsync(user))[0];
            Assert.AreEqual(Role.Admin.ToString(), result);
        }

        [Test]
        public async Task SaveUsersToXlsx_WithCorrectParams_SavesUsersToFile()
        {
            await _adminService.SaveUsersToXlsxAsync(FileName);

            using (var package =
                new ExcelPackage(
                    new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), FileName + ".xlsx"))))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                Assert.AreEqual("admin", worksheet.Cells[1, 1].Value.ToString());
                Assert.AreEqual(0, int.Parse(worksheet.Cells[1, 2].Value.ToString()));
            }
        }
    }
}