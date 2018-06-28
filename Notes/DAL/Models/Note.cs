using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notes.DAL.Models
{
    public class Note
    {
        [Key] public int Id { get; set; }
        [ForeignKey("UserId")] public User User { get; set; }
        public byte[] Identicon { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
    }
}