using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginBackend2023.Models
{
    [Index(nameof(Email), nameof(Link), IsUnique = true)]
    public class Favoritos
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Link { get; set; }
       

    }

}
