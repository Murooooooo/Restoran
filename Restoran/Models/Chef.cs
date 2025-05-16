using System.ComponentModel.DataAnnotations;

namespace Restoran.Models
{
    public class Chef
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }
        [Required]
        [MaxLength(50)]
        public string Position { get; set; }
        
        public string? PhotoUrl { get; set; }

    }
}
