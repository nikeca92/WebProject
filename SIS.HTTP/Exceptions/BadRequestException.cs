using System;

namespace SIS.HTTP.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException()
        {
            
        }
        public BadRequestException(string name) : base(name)
        {

        }
    }
}
