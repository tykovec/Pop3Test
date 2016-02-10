using System;
using log4net;
using Test.Pop3;

namespace Test
{
    class Program
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            //if (args.Length == 0)
            //    return;

           Downloader.Download();


           Console.ReadKey();
        }
    }
}
