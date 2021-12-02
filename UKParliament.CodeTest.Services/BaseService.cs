#region Namespace References
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UKParliament.CodeTest.Pattern.Service.Interfaces;
using UKParliament.CodeTest.Pattern.WebAPI.SmartWrapper;
#endregion

namespace UKParliament.CodeTest.Services
{
    public abstract class BaseService<TType, TResult>
    {
        protected BaseService()
        { }

        protected BaseService(ISmartServiceFactory serviceFactory,
            ILogger<TType> logger)
        {
            Logger = logger;
            ServiceFactory = serviceFactory;
        }

        /// <summary>
        /// Logger to be inherited.
        /// </summary>
        protected ILogger<TType> Logger { get; set; }
        protected ISmartServiceFactory ServiceFactory { get; set; }

        /// <summary>
        /// Exception Logging
        /// </summary>
        /// <param name="ex">Exception</param>
        protected void LogException(Exception ex)
        {
            Logger.LogError(ex.Message, ex);
        }


        protected ApiResponse<TResult> GenerateApiResponse(
            TResult value,
            bool hasError = false,
            string message = "",
            HttpStatusCode statusCode = HttpStatusCode.BadRequest,
            string exceptionMessage = "")
        {
            if (EqualityComparer<TResult>.Default.Equals(value, default(TResult)))
                return value as ApiResponse<TResult>;
            return new ApiResponse<TResult>
            {
                StatusCode = hasError ? (int)statusCode : (int)HttpStatusCode.OK,
                IsError = hasError,
                Message = hasError ? message : "Success",
                ExceptionMessage = hasError ? exceptionMessage : string.Empty,
                Result = hasError ? default :  value
            };
        }

        protected ApiResponse<TResult> GenerateApiResponse(
            List<TResult> value,
            bool hasError = false,
            string message = "",
            HttpStatusCode statusCode = HttpStatusCode.BadRequest,
            string exceptionMessage = "")
        {
            if (EqualityComparer<List<TResult>>.Default.Equals(value, default))
                return value as ApiResponse<TResult>;

            return new ApiResponse<TResult>
            {
                StatusCode = hasError ? (int)statusCode : (int)HttpStatusCode.OK,
                IsError = hasError,
                Message = hasError ? message : "Success",
                ExceptionMessage = hasError ? exceptionMessage : string.Empty,
                Results = hasError ? default :  (List<TResult>)value.AsEnumerable()
            };
        }
    }
}
