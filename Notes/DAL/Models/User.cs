using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Notes.DAL.Models
{
    public class User : IdentityUser<long>
    {
    }
}