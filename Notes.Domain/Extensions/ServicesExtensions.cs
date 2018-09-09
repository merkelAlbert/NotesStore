using Microsoft.Extensions.DependencyInjection;
using Notes.Domain.Interfaces;
using Notes.Domain.Services;

namespace Notes.Domain.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<INotesService, NotesService>();
            services.AddScoped<IIdenticonService, IdenticonService>();
            services.AddScoped<IXlsxService, XlsxService>();
            return services;
        }
    }
}