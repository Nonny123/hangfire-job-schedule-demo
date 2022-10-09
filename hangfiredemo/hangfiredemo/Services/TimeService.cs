using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hangfiredemo.Services
{
    public interface ITimeService
    {
        void PrintNow();
    }

    public class TimeService : ITimeService
    {
        private readonly ILogger<TimeService> logger;

        public TimeService(ILogger<TimeService> logger)
        {
            this.logger = logger;
        }

        public void PrintNow()
        {
            logger.LogInformation(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
        }
    }
}
