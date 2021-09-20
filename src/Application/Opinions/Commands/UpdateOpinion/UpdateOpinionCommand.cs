﻿using System;
using MediatR;

namespace Application.Opinions.Commands.UpdateOpinion
{
    /// <summary>
    ///     Update opinion command
    /// </summary>
    public class UpdateOpinionCommand : IRequest
    {
        /// <summary>
        ///     Opinion ID
        /// </summary>
        public Guid OpinionId { get; set; }

        /// <summary>
        ///     Opinion rate
        /// </summary>
        public int Rate { get; set; }

        /// <summary>
        ///     Opinion comment
        /// </summary>
        public string Comment { get; set; }
    }
}