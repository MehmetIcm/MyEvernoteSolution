using System;
using System.Collections.Generic;
using System.Text;

namespace MyEvernote.Entities
{
    //For M2M Relation
    public class Liked
    {
        public int Id { get; set; }
        public virtual Note Note { get; set; }
        public virtual EvernoteUser LikedUser { get; set; }
    }
}
