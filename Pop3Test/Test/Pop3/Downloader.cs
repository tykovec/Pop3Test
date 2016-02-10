using System;
using System.Net.Mail;
using log4net;
using OpenPop.Pop3;
using Npgsql;
using NpgsqlTypes;
using Test.Db;

namespace Test.Pop3
{
    public static class Downloader
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Downloader));

        public static void Download()
        {

            var pop3Settings = Properties.Settings.Default.Pop3;
            var dbSettings = Properties.Settings.Default.Db;


            using (var client = new Pop3Client())
            {
                try
                {

                    // Connect to the server
                    client.Connect(pop3Settings.HostName, pop3Settings.Port, pop3Settings.UseSSL);

                    // Authenticate ourselves towards the server
                    client.Authenticate(pop3Settings.UserName, pop3Settings.Password);

                    // Get the number of messages in the inbox
                    var messageCount = client.GetMessageCount();

                    if (messageCount == 0)
                        return;

                    using (var connection = new NpgsqlConnection(dbSettings.ConnectionString))
                    {
                        connection.Open();

                        using (var cmd = new NpgsqlCommand($"insert into {dbSettings.TableName} (From, To, Subject, Body, Status) values (@From, @To, @Subject, @Body, @Status)", connection))
                        {
                            // Messages are numbered in the interval: [1, messageCount]
                            // Ergo: message numbers are 1-based.
                            // Most servers give the latest message the highest number
                            for (int i = messageCount; i > 0; i--)
                            {
                                var message = client.GetMessage(i).ToMailMessage();

                                _logger.Debug($"Retrieve message {i}");

                                cmd.AddParameter("@From", NpgsqlDbType.Char, message.From);
                                cmd.AddParameter("@To", NpgsqlDbType.Char, message.To);
                                cmd.AddParameter("@Subject", NpgsqlDbType.Char, message.Subject);
                                cmd.AddParameter("@Body", NpgsqlDbType.Text, message.Body);
                                cmd.AddParameter("@Status", NpgsqlDbType.Text, dbSettings.Status);

                                cmd.ExecuteNonQuery();

                                //_logger.Debug($"Delete message {i}");
                                //client.DeleteMessage(i);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    _logger.Error("Exception on download", ex);
                }
            }
        }
    }
}