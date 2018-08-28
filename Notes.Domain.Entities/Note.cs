using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Notes.Domain.Entities
{
    public class Note
    {
        [Key] public int Id { get; set; }
        [ForeignKey("UserId")] public IdentityUser User { get; set; }
        public byte[] Identicon { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
    }
}