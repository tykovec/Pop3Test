using System;
using Quartz;
using Quartz.Impl;
using Test.Jobs;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
           JobRunner.RunJobs();

           Console.ReadKey();
        }
    }
}
