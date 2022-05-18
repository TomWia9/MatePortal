using System;

namespace Domain.Common
{
    /// <summary>
    ///     Base entity
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        ///     Date of creation
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        ///     Id of the creator
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        ///     Date of last modification
        /// </summary>
        public DateTime? LastModified { get; set; }

        /// <summary>
        ///     Id of the last modificator
        /// </summary>
        public Guid? LastModifiedBy { get; set; }
    }
}