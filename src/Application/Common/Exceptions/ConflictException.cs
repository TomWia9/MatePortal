using System;

namespace Application.Common.Exceptions
{
    /// <summary>
    ///     Conflict exception
    /// </summary>
    public class ConflictException : Exception
    {
        /// <summary>
        ///     Initializes default ConflictException
        /// </summary>
        public ConflictException()
        {
        }

        /// <summary>
        ///     Initializes ConflictException with name and key of entity
        /// </summary>
        /// <param name="name">Entity name</param>
        public ConflictException(string name) : base(
            $"Entity \"{name}\" conflicts with another entity")
        {
        }
    }
}