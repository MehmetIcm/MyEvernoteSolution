using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyEvernote.Entities
{
    [Table("EvernoteUsers")]
    public class EvernoteUser : MyEntityBase
    {
        [DisplayName("İsim"), 
            StringLength(25, ErrorMessage = "{0} alanı maksimum {1} karakter olmalıdır")]
        public string Name { get; set; }
        [DisplayName("Soyad"), 
            StringLength(25, ErrorMessage = "{0} alanı maksimum {1} karakter olmalıdır")]
        public string Surname { get; set; }
        [DisplayName("Kullanıcı Adı"), 
            Required(ErrorMessage = "{0} alan gereklidir."), 
            StringLength(25, ErrorMessage = "{0} alanı maksimum {1} karakter olmalıdır")]
        public string Username { get; set; }
        [DisplayName("E-Posta"), 
            Required(ErrorMessage = "{0} alan gereklidir."), 
            StringLength(80, ErrorMessage = "{0} alanı maksimum {1} karakter olmalıdır")]
        public string Email { get; set; }
        [DisplayName("Şifre"), 
            Required(ErrorMessage = "{0} alan gereklidir."), 
            StringLength(25, ErrorMessage = "{0} alanı maksimum {1} karakter olmalıdır")]
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
