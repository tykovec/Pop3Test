using System;
using System.Collections.Generic;
using System.Diagnostics;
using log4net;
using OpenPop.Mime;
using OpenPop.Pop3;
using Quartz;

namespace Test.Jobs
{
    [DisallowConcurrentExecution]
    public class RetrievePop3DataJob : IJob
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(RetrievePop3DataJob));

        public void Execute(IJobExecutionContext context)
        {

            _logger.Debug("RetrievePop3DataJob");

            var settings = Properties.Settings.Default.Pop3Settings;


            using (var client = new Pop3Client())
            {
                try
                {
                    // Connect to the server
                    client.Connect(settings.HostName, settings.Port, settings.UseSSL);

                    // Authenticate ourselves towards the server
                    client.Authenticate(settings.UserName, settings.Password);

                    // Get the number of messages in the inbox
                    int messageCount = client.GetMessageCount();

                    // Messages are numbered in the interval: [1, messageCount]
                    // Ergo: message numbers are 1-based.
                    // Most servers give the latest message the highest number
                    for (int i = messageCount; i > 0; i--)
                    {
                        var message = client.GetMessage(i).ToMailMessage();

                        _logger.Debug($"Retrieve message {i}");

                        //client.DeleteMessage(i);
                        //_logger.Debug($"Delete message {i}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(this, ex);
                }
            }
        }
    }
}