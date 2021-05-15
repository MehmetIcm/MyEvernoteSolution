using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyEvernote.Entities
{
    [Table("Categories")]
    public class Category:MyEntityBase
    {
        [DisplayName("Başlık"),Required, StringLength(50)]
        public string Title { get; set; }
        [DisplayName("Açıklama"), StringLength(200)]
        public string Description { get; set; }
        public virtual List<Note> Notes { get; set; } //The property realtional with another class so we defined as virtual 

        //i don't want to take null exception When i want to add new note
        public Category()
        {
            Notes = new List<Note>();
        }
    }
}
