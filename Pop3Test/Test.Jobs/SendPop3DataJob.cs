using log4net;
using Quartz;

namespace Test.Jobs
{
    [DisallowConcurrentExecution]
    public class SendPop3DataJob : IJob
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(RetrievePop3DataJob));

        public void Execute(IJobExecutionContext context)
        {
            _logger.Debug("SendPop3Data");
        }
    }
}