using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyEvernote.DataAccessLayer;

namespace MyEvernote.BusinessLayer
{
    public class Test
    {
        //It's for calling database creator initializer
        public Test()
        {
            DatabaseContext db = new DatabaseContext();
            db.Categories.ToList();
        }
    }
}
