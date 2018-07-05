using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Entities;
using System.Configuration;
using System.Xml.Serialization;
using System.Reflection;

namespace MedianConsumption
{
    class Program
    {
        

        static void Main(string[] args)
        {
            try
            {
                

                #region Fetching the XML File Configuration

                string fileTypeXMLpath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"FileConfiguration.xml");
                FileTypes fileTypes = new FileTypes();

                XmlSerializer serializer = new XmlSerializer(typeof(FileTypes));
                using (FileStream fileStream = new FileStream(fileTypeXMLpath, FileMode.Open))
                {
                    fileTypes = (FileTypes)serializer.Deserialize(fileStream);
                }

                #endregion

                #region Fetching the input files in the input location

                string folderPath = ConfigurationManager.AppSettings["InputFolder"];
                List<string> inputFiles =  new List<string>();
                List<DataFile> dataFiles = new List<DataFile>();

                foreach (FileType fileType in fileTypes.FileType)
                {
                    if (inputFiles.Count() == 0)                    
                        inputFiles = Directory.GetFiles(folderPath, fileType.Identifier).ToList();
                    else
                        inputFiles = inputFiles.Concat(Directory.GetFiles(folderPath, fileType.Identifier).ToList()).ToList();

                    foreach (string inputFile in Directory.GetFiles(folderPath, fileType.Identifier).ToList())
                    {
                        DataFile dataFile = new DataFile();
                        dataFile.FileName = inputFile.Substring(inputFile.LastIndexOf("\\"), (inputFile.Length - inputFile.LastIndexOf("\\")));
                        dataFile.FileType = fileType.Identifier;
                        dataFiles.Add(dataFile);
                    }
                }
                
                #endregion

                #region Validate and Process

                if (inputFiles.Count() == 0)                
                    Console.WriteLine("No Files Found in Input Directory! Exiting processing. ");                
                //Processing the Input Files
                else
                     ProcessInputFiles(dataFiles, fileTypes, folderPath);
                #endregion

                Console.WriteLine("Read Completed.");
                Console.Read();
            }

            catch(Exception ex)
            {
                Console.WriteLine(ex);
                Console.Read();
            }
        }

        /// <summary>
        /// Processes a list of datafiles
        /// </summary>
        /// <param name="dataFiles"></param>
        /// <param name="fileTypes"></param>
        /// <param name="folderPath"></param>
        public static void ProcessInputFiles(List<DataFile> dataFiles, FileTypes fileTypes, string folderPath)
        {
            try
            {
                
                List<DataFile> dataFileList = new List<DataFile>();
                foreach (DataFile dataFile  in dataFiles)
                {
                   
                    Console.WriteLine("Processing "+ dataFile.FileName);

                    #region File Validation
                    //Check Blank files
                    if (new FileInfo(folderPath+ dataFile.FileName).Length == 0)
                    {
                        Console.WriteLine(dataFile.FileName +" is blank, nothing to read! ");
                        ArchiveProcessedFile(dataFile.FileName, folderPath);
                        continue;
                    }

                    bool headerValidationSuccess = true;
                    string[] csvLines=  File.ReadAllLines(folderPath+ dataFile.FileName);
                    string[] headers = csvLines[0].Split(',');

                    //Get the Column Names for the files and check if they contain the required column names, and check the indexes

                    FileType fileType = fileTypes.FileType.FirstOrDefault(x => x.Identifier == dataFile.FileType);

                    foreach (Item item in fileType.Item)
                    {
                        item.Index = null;
                        for(int i =0;i<headers.Count();i++)
                        {
                            if (headers[i] == item.FileHeaderName)
                            {
                                item.Index = i;
                                break;
                            }
                        }

                        if (item.Index == null)
                        {
                            Console.WriteLine("Column " + item.FileHeaderName + " is missing in file");
                            headerValidationSuccess = false;
                            break;
                            
                        }
                    }


                    #endregion

                    #region Read data from the file and calculate median values
                    if (headerValidationSuccess)
                    {
                        List<FileRow> meterReads = new List<FileRow>();

                        foreach (string csvLine in csvLines.Skip(1))
                        {

                            string[] values = csvLine.Split(',');


                            // Validation to check if the data is in the appropriate format

                            int dateTimeColumnIndex = Convert.ToInt32(fileType.Item.FirstOrDefault(x => x.ValueType == "Occurence DateTime").Index);
                            int valueColumnIndex = Convert.ToInt32(fileType.Item.FirstOrDefault(x => x.ValueType == "Data Value").Index);

                            if (DateTime.TryParse(values[dateTimeColumnIndex], out DateTime tempDateTime))
                                tempDateTime = Convert.ToDateTime(values[dateTimeColumnIndex]);
                            else
                                continue;

                            if (Double.TryParse(values[valueColumnIndex], out Double tempDouble))
                                tempDouble = Convert.ToDouble(values[valueColumnIndex]);
                            else
                                continue;

                            FileRow meterRead = new FileRow(tempDateTime, tempDouble);
                            meterReads.Add(meterRead);

                        }

                        double median = DataFile.CalculateMedian(meterReads);

                        foreach (FileRow meterRead in meterReads)
                        {
                            if (meterRead.DataValue > (1.2 * median))
                                meterRead.Divergence = Enums.Divergence.GreaterThanTwentyPercent;

                            else if (meterRead.DataValue < (0.8 * median))
                                meterRead.Divergence = Enums.Divergence.LessThanTwentyPercent;

                            if ((meterRead.Divergence == Enums.Divergence.LessThanTwentyPercent) || (meterRead.Divergence == Enums.Divergence.GreaterThanTwentyPercent))
                                Console.WriteLine(dataFile.FileName + " " + meterRead.OccurenceDateTime.ToString() + " "
                                    + meterRead.DataValue.ToString()
                                    + " " + median.ToString());
                        }

                        dataFile.MedianValue = median;
                        dataFile.MeterReads = meterReads;
                        dataFileList.Add(dataFile);
                    }

                    #endregion

                    ArchiveProcessedFile(dataFile.FileName, folderPath);
                    
                }

                OutputStatistics(dataFileList);
                
            }

            catch(Exception ex)
            {
                throw ex;
            }
        }
       
        /// <summary>
        /// Archives a file to a archive directory
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="folderPath"></param>
        public static void ArchiveProcessedFile(string fileName, string folderPath)
        {
            try
            {
                string archivePath = folderPath + "\\Archive"+ fileName;
                string sourcePath = folderPath + fileName;
                Directory.CreateDirectory(folderPath+ "\\Archive");
                if(!File.Exists(archivePath))
                   File.Move(sourcePath, archivePath);
                else
                {
                    File.Delete(archivePath);
                    File.Move(sourcePath, archivePath);
                }
            }

            catch(Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Prints Statistics at the end of processing
        /// </summary>
        /// <param name="datafiles"></param>
        public static void OutputStatistics(List<DataFile> datafiles)
        {
            try
            {
                Console.WriteLine("------------Processing Statistics--------------");

                foreach (DataFile dataFile in datafiles)
                {


                    Console.WriteLine("------------File Processed Name: " + dataFile.FileName + " ----------------------");
                    Console.WriteLine("Total Number of Rows Processed: " + dataFile.MeterReads.Count().ToString());
                    Console.WriteLine("Number of Rows divergent from median by more than 20%: " +
                        dataFile.MeterReads.FindAll(x => x.Divergence == Enums.Divergence.GreaterThanTwentyPercent).Count().ToString());
                    Console.WriteLine("Number of Rows divergent from median by less than 20%: " +
                        dataFile.MeterReads.FindAll(x => x.Divergence == Enums.Divergence.LessThanTwentyPercent).Count().ToString());
                    Console.WriteLine("-------------------------------------------------------------------------------------------------");


                }
            }

            catch(Exception ex)
            {
                throw ex;
            }

        }
    }
}
