using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace UKParliament.CodeTest.Web.Controllers
{
    public abstract class BaseController<T> : ControllerBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">Inject logger</param>
        /// <param name="configuration"></param>
        protected BaseController(ILogger<T> logger)
        {
            Logger = logger;
        }

        /// <summary>
        /// Logger to be inherited.
        /// </summary>
        protected ILogger<T> Logger { get; set; }

        /// <summary>
        /// Exception Logging
        /// </summary>
        /// <param name="ex">Exception</param>
        protected void LogException(Exception ex)
        {
            Logger.LogError(ex.Message, ex);
        }
    }
}
