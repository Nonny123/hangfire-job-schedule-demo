using Hangfire;
using Hangfire.Server;
using hangfiredemo.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hangfiredemo.Services
{
    public interface IPeopleRepository
    {
        Task CreatePerson(string personName);
        void DeploySurvey(PerformContext context);

        
    }

    public class PeopleRepository : IPeopleRepository
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<PeopleRepository> logger;

        public PeopleRepository(ApplicationDbContext context, ILogger<PeopleRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task CreatePerson(string personName)
        {
            logger.LogInformation($"Adding person {personName}");
            var person = new Person { Name = personName };
            context.Add(person);
            await Task.Delay(5000);
            await context.SaveChangesAsync();
            logger.LogInformation($"Added the person {personName}");
        }

        //[AutomaticRetry(Attempts = 0)]
        public void DeploySurvey(PerformContext context)
        {
            int timeInMinutes = 3;


            SpoolData();

            BackgroundJob.ContinueJobWith(context.BackgroundJob.Id, () => RetrieveCreateMailList());


            BackgroundJob.Schedule(() => SendMailSurvey(), TimeSpan.FromSeconds(timeInMinutes));

        }


        public void SpoolData()
        {
            logger.LogInformation($"Spooled Data");
        }


        public void RetrieveCreateMailList()
        {
            logger.LogInformation($"retrieved and created email list");
        }


        public void SendMailSurvey()
        {
            logger.LogInformation($"Sent Mail Survey");
        }

    }
}
