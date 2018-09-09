using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Notes.Domain.Models;

namespace Notes.Domain.Interfaces
{
    public interface IAccountService
    {
        Task<bool> UserExistsAsync(string email);
        Task<IdentityResult> RegisterAsync(RegisterViewModel model);
        Task<SignInResult> SignInAsync(LoginViewModel model);
        Task SignOutAsync();
    }
}