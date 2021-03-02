﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyEvernote.Entities
{
    public class Category:MyEntityBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public virtual List<Note> Notes { get; set; } //The property realtional with another class so we defined as virtual 
    }
}
