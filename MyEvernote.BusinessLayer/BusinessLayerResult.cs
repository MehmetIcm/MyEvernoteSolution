using MyEvernote.Entities.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyEvernote.BusinessLayer
{
    public class BusinessLayerResult<T> where T:class
    {
        //We want to keep Errors Messages, if we don't have any error we want to keep Result
        public List<ErrorMessageObj> Errors { get; set; }    
        public T Result { get; set; }

        public BusinessLayerResult()
        {
            Errors = new List<ErrorMessageObj>();
        }

        public void AddError(ErrorMessageCode code, string message)
        {
            Errors.Add(new ErrorMessageObj(){Code=code, Message=message});
        }
    }
}
