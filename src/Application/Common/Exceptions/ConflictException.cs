using System;

namespace Application.Common.Exceptions
{
    /// <summary>
    /// Conflict exception
    /// </summary>
    public class ConflictException : Exception
    {
        /// <summary>
        /// Initializes default ConflictException
        /// </summary>
        public ConflictException()
        {
        }

        /// <summary>
        /// Initializes ConflictException with message
        /// </summary>
        /// <param name="message">The message</param>
        public ConflictException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes ConflictException with name and key of entity
        /// </summary>
        /// <param name="name">Entity name</param>
        /// <param name="key">Entity key</param>
        public ConflictException(string name, object key) : base(
            $"Entity \"{name}\" ({key}) conflicts with another entity")
        {
        }
    }
}