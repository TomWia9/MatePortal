using System;
using System.Collections.Generic;

namespace Application.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }
        
        public ValidationException() : base("One or more validation failures have occured")
        {
            Errors = new Dictionary<string, string[]>();
        }
        
        //TODO Add second constructor with failures from validator when fluent validation will be added
    }
}