using HR.LeaveManagement.Application.Contracts.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Infrastructure.Logging
{
    public class LoggerAdapter<T> : IAppLogger<T>
    {
        private readonly ILogger _iLogger;

        public LoggerAdapter(ILoggerFactory loggerFactory )
        {
            this._iLogger = loggerFactory.CreateLogger<T>();
        }

        public void LogInformation(string message, params object[] args)
        {
            _iLogger.LogInformation(message, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            _iLogger.LogWarning(message, args);
        }
    }
}
