using Microsoft.Extensions.DependencyInjection;
using Notes.Domain.Services;

namespace Notes.Domain.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<AccountService>();
            services.AddScoped<AdminService>();
            services.AddScoped<NotesService>();
            services.AddScoped<IdenticonService>();
            services.AddScoped<XlsxService>();
            return services;
        }
    }
}