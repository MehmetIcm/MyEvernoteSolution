using System;
using System.Collections.Generic;
using System.Text;

namespace MyEvernote.Entities.Messages
{
    public class ErrorMessageObj
    {
        public ErrorMessageCode Code { get; set; }
        public string Message { get; set; }
    }
}
