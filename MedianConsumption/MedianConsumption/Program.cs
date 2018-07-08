using System;
using System.Linq;
using Entities;
using System.Configuration;
using System.Reflection;
using System.Collections.Generic;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace MedianConsumption
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            try
            {
                string folderPath = ConfigurationManager.AppSettings["InputFolder"];
                IFileProcessor fileProcessor = new FileProcessor();
                IArchivalHandler archivalHandler = new ArchivalHandler();
                List<DataFile> dataFiles = new List<DataFile>();
                fileProcessor.ProcessAllFiles(folderPath, archivalHandler, ref dataFiles);
                Console.Read();
            }
            catch(Exception ex)
            {
                log.Fatal(ex);
                Console.Read();
            }
        }
    }
}
