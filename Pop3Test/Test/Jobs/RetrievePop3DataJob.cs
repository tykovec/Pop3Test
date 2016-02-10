using Quartz;
using Test.Pop3;

namespace Test.Jobs
{
    [DisallowConcurrentExecution]
    public class RetrievePop3DataJob : IJob
    {

        public void Execute(IJobExecutionContext context)
        {
            Downloader.Download();
        }
    }
}