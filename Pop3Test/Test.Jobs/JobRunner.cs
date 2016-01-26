using System;
using log4net;
using Quartz;
using Quartz.Impl;

namespace Test.Jobs
{
    public class JobRunner
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(RetrievePop3DataJob));

        public static void RunJobs()
        {
            // construct a scheduler factory
            ISchedulerFactory schedFact = new StdSchedulerFactory();

            // get a scheduler
            IScheduler sched = schedFact.GetScheduler();
            sched.Start();

            // create trigger
            var retrieveJob = JobBuilder.Create<RetrievePop3DataJob>().WithIdentity("retrieve pop3 group", "mail task group").Build();
            var sendJob = JobBuilder.Create<SendPop3Data>().WithIdentity("send pop3 group", "mail task group").Build();

            var interval = Properties.Settings.Default.Interval;

            _logger.Debug($"Starting jobs with interval {interval}");

            sched.ScheduleJob(retrieveJob, CreateTrigger($"every {interval} seconds trigger", interval));
            sched.ScheduleJob(sendJob, CreateTrigger($"every {interval} seconds delayed", interval, 3));

            Console.ReadKey();
        }

        private static ITrigger CreateTrigger(string name, int interval, int secondsDelay = 0)
        {
            var tb = TriggerBuilder.Create()
                .WithIdentity(name, "mail task group")
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(interval).RepeatForever().WithMisfireHandlingInstructionIgnoreMisfires());
            if (secondsDelay == 0)
                tb.StartNow();
            else
                tb.StartAt(DateTimeOffset.Now.AddSeconds(secondsDelay));

            return tb.Build();
        }
    }
}