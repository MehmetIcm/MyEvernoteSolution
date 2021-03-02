using System;
using System.Collections.Generic;
using System.Text;

namespace MyEvernote.Entities
{
    public class MyEntityBase
    {
        public string Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedUsername { get; set; }
    }
}
