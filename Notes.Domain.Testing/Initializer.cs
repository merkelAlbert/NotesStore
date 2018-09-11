using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Notes.Database;
using Notes.Domain.Extensions;
using Notes.Domain.Interfaces;
using Notes.Domain.Testing.Services;
using Notes.Domain.Testing.Wrappers;
using Notes.Domain.Utils;
using NUnit.Framework;

namespace Notes.Domain.Testing
{
    [SetUpFixture]
    public class Initializer
    {
        public static IServiceProvider Provider { get; private set; }

        [OneTimeSetUp]
        public void Init()
        {
            var services = new ServiceCollection();
            services.AddDomainServices();
            services.AddDbContext<DatabaseContext>(options => { options.UseInMemoryDatabase("Notes"); });
            services.AddScoped<IIdenticonService, FakeIdenticonService>();
            services.AddScoped<NotesWrapper>();
            services.AddScoped<RolesInitializer>();

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                })
                .AddEntityFrameworkStores<DatabaseContext>();

            Provider = services.BuildServiceProvider();
        }
    }
}