using System;
using System.Collections.Generic;
using System.Text;

namespace MyEvernote.Entities
{
    public class Comment:MyEntityBase
    {
        public string Text { get; set; }
        public virtual Note Note { get; set; }
        public virtual EvernoteUser Owner { get; set; }
    }
}
