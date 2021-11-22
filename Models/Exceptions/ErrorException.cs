using System;

namespace pfm.Models.Exceptions
{
    public class ErrorException : Exception
    {
        public ValidationProblem ValidationProblem { get; set; }
        public ErrorException(ValidationProblem validationProblem)
        {
            this.ValidationProblem = validationProblem;
        }
    }
}