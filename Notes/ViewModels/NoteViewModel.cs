using System.ComponentModel.DataAnnotations;

namespace Notes.ViewModels
{
    public class NoteViewModel
    {
        public int Id { get; set; }
        public byte[] Identicon { get; set; }
        
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Text { get; set; }
    }
}