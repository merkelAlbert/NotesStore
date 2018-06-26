using System.ComponentModel.DataAnnotations;

namespace Notes.DAL.Models
{
    public class User
    {
        [Key] public int Id { get; set; }
        public int Login { get; set; }
        public int Password { get; set; }
    }
}