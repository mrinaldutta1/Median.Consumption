using System;
using System.Linq;
using Entities;
using System.Configuration;
using System.Reflection;

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
                fileProcessor.ProcessAllFiles(folderPath);
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
