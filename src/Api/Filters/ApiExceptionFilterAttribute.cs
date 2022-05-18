using System;
using System.Collections.Generic;
using Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Api.Filters
{
    /// <summary>
    ///     The Api Exception filter attribute
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        ///     The collection of exception handlers
        /// </summary>
        private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

        /// <summary>
        ///     The logger
        /// </summary>
        private readonly ILogger<ApiExceptionFilterAttribute> _logger;

        /// <summary>
        ///     Initializes ApiExceptionFilterAttribute
        /// </summary>
        /// <param name="logger">The logger</param>
        public ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> logger)
        {
            _logger = logger;
            _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                {typeof(ValidationException), HandleValidationException},
                {typeof(NotFoundException), HandleNotFoundException},
                {typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException},
                {typeof(ForbiddenAccessException), HandleForbiddenAccessException},
                {typeof(ConflictException), HandleConflictException}
            };
        }

        /// <summary>
        ///     Handles exception
        /// </summary>
        /// <param name="context">The exception context</param>
        public override void OnException(ExceptionContext context)
        {
            HandleException(context);
            base.OnException(context);
        }

        /// <summary>
        ///     Handles exception
        /// </summary>
        /// <param name="context">The exception context</param>
        private void HandleException(ExceptionContext context)
        {
            var type = context.Exception.GetType();
            if (_exceptionHandlers.ContainsKey(type))
            {
                _exceptionHandlers[type].Invoke(context);
                return;
            }

            if (!context.ModelState.IsValid)
            {
                HandleInvalidModelStateException(context);
                return;
            }

            HandleUnknownException(context);
        }

        /// <summary>
        ///     Handles validation exception
        /// </summary>
        /// <param name="context">The exception context</param>
        private void HandleValidationException(ExceptionContext context)
        {
            var exception = context.Exception as ValidationException;

            var details = new ValidationProblemDetails(exception?.Errors)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            if (exception != null) _logger.LogError(exception.Message);

            context.Result = new BadRequestObjectResult(details);
            context.ExceptionHandled = true;
        }

        /// <summary>
        ///     Handles NotFound exception
        /// </summary>
        /// <param name="context">The exception context</param>
        private void HandleNotFoundException(ExceptionContext context)
        {
            var exception = context.Exception as NotFoundException;

            var details = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Title = "The specified resource was not found.",
                Detail = exception?.Message
            };

            if (exception != null) _logger.LogError(exception.Message);

            context.Result = new NotFoundObjectResult(details);
            context.ExceptionHandled = true;
        }

        /// <summary>
        ///     Handles UnauthorizedAccess exception
        /// </summary>
        /// <param name="context">The exception context</param>
        private void HandleUnauthorizedAccessException(ExceptionContext context)
        {
            var details = new ProblemDetails
            {
                Title = "Unauthorized",
                Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
            };


            _logger.LogError(context.Exception.Message);

            context.Result = new UnauthorizedObjectResult(details);
            context.ExceptionHandled = true;
        }

        /// <summary>
        ///     Handles ForbiddenAccess exception
        /// </summary>
        /// <param name="context">The exception context</param>
        private void HandleForbiddenAccessException(ExceptionContext context)
        {
            var details = new ProblemDetails
            {
                Status = StatusCodes.Status403Forbidden,
                Title = "Forbidden",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3"
            };

            _logger.LogError(context.Exception.Message);

            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status403Forbidden
            };

            context.ExceptionHandled = true;
        }

        /// <summary>
        ///     Handles Conflict exception
        /// </summary>
        /// <param name="context">The exception context</param>
        private void HandleConflictException(ExceptionContext context)
        {
            var exception = context.Exception as ConflictException;

            var details = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                Title = "The specified resource conflict with another resource.",
                Detail = exception?.Message
            };

            if (exception != null) _logger.LogError(exception.Message);

            context.Result = new ConflictObjectResult(details);
            context.ExceptionHandled = true;
        }

        /// <summary>
        ///     Handles InvalidModelState exception
        /// </summary>
        /// <param name="context">The exception context</param>
        private void HandleInvalidModelStateException(ExceptionContext context)
        {
            var details = new ValidationProblemDetails(context.ModelState)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            _logger.LogError(context.Exception.Message);

            context.Result = new BadRequestObjectResult(details);
            context.ExceptionHandled = true;
        }

        /// <summary>
        ///     Handles Unknown exception
        /// </summary>
        /// <param name="context">The exception context</param>
        private void HandleUnknownException(ExceptionContext context)
        {
            var details = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An error occurred while processing your request.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            };

            _logger.LogError(context.Exception.Message);

            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context.ExceptionHandled = true;
        }
    }
}