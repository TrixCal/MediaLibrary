using System;
using System.IO;
using NLob.Web;

namespace MediaLibrary
{
    class Program
    {
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        static void Main(string[] args)
        {
            logger.Info("Program Started");

            Console.WriteLine("Hello World!");

            logger.Info("Program Ended");
        }
    }
}
