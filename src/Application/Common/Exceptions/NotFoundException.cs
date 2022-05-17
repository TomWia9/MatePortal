using System;

namespace Application.Common.Exceptions
{
    /// <summary>
    /// Not found exception
    /// </summary>
    public class NotFoundException : Exception
    {
        /// <summary>
        /// Initializes default NotFoundException
        /// </summary>
        public NotFoundException()
        {
        }

        /// <summary>
        /// Initializes NotFoundException with message
        /// </summary>
        /// <param name="message">Entity name</param>
        public NotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes NotFoundException with entity name and key
        /// </summary>
        /// <param name="name">Entity name</param>
        /// <param name="key">Entity key</param>
        public NotFoundException(string name, object key) : base($"Entity \"{name}\" ({key}) was not found")
        {
        }
    }
}