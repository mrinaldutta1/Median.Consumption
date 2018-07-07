using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Entities;
using System.Configuration;
using System.Xml.Serialization;
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
                log.Debug("Start Main()");
                var fileTypes = FileTypes.GetFileTypes();
                string folderPath = ConfigurationManager.AppSettings["InputFolder"];

                IFileProcessor fileProcessor = new FileProcessor();
                var dataFiles = fileProcessor.FetchAllDataFiles(fileTypes, folderPath);                

                if (dataFiles.Count() == 0)
                    log.Warn("No Files Found in Input Directory, Exiting Processing!");
                else
                {
                    double divergencePercentage = Convert.ToDouble(ConfigurationManager.AppSettings["DivergencePercentage"]);
                    foreach (DataFile dataFile in dataFiles)
                    {
                        try
                        {
                            FileProcessStatus fileProcessStatus = FileProcessStatus.Undetermined;
                            fileProcessStatus = fileProcessor.ProcessInputFile(dataFile, fileTypes, folderPath, divergencePercentage);
                            fileProcessor.ProcessFileArchival(fileProcessStatus, dataFile.FileName, folderPath);
                        }
                        catch(Exception ex)
                        {
                            log.Error(ex);
                        }
                        
                    }
                    fileProcessor.OutputStatistics(dataFiles, divergencePercentage);
                }              

                
                log.Info("Read Completed");
                Console.Read();
                log.Debug("End Main()");
            }

            catch(Exception ex)
            {
                log.Fatal(ex);
                Console.Read();
            }
        }
    }
}
