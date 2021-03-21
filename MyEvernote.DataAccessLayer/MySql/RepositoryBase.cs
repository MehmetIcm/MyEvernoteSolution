using System;
using System.Collections.Generic;
using System.Text;

namespace MyEvernote.DataAccessLayer.MySql
{
    public class RepositoryBase
    {
        protected static object context;
        private static object _lockSync = new object();
        protected RepositoryBase()
        {
            CreateContext();
        }

        private static void CreateContext()
        {
            if (context == null)
            {
                lock (_lockSync) //Multithred uygulamalarda aynı anda iki kod çalışmamasını istediğimiz için lock kullanırız
                {
                    if (context == null)
                    {
                        context = new object();
                    }
                }
            }
        }
    }
}
