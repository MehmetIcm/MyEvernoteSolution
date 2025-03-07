﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyEvernote.Entities
{
    //For M2M Relation
    [Table("Likes")]
    public class Liked
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual Note Note { get; set; }
        public virtual EvernoteUser LikedUser { get; set; }
    }
}
