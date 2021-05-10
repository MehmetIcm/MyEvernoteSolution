using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyEvernote.Entities
{
    [Table("EvernoteUsers")]
    public class EvernoteUser:MyEntityBase
    {
        [StringLength(25)]
        public string Name { get; set; }
        [StringLength(25)]
        public string Surname { get; set; }
        [StringLength(25)]
        public string Username { get; set; }
        [Required,StringLength(80)]
        public string Email { get; set; }
        [Required,StringLength(25)]
        public string Password { get; set; }
        [StringLength(150)]
        public string ProfileImageFileName { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
        public virtual List<Note> Notes { get; set; }
        [Required]
        public Guid ActivateGuid { get; set; } //for email activating link 
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }
    }
}
