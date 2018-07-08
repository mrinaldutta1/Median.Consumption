using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using System.Reflection;
using System.Configuration;

namespace MedianConsumption
{
    /// <summary>
    /// Implements the IFileProcessor interface
    /// </summary>
    public class FileProcessor:IFileProcessor
    {
        private readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public bool ProcessAllFiles(string  folderPath)
        {
            try
            {
                log.Debug("Start ProcessAllFiles()");
                var fileTypes = FileTypes.GetFileTypes();
                
               
                var dataFiles = FetchAllDataFiles(fileTypes, folderPath);
                bool atleastOneFileFound =true;

                if (dataFiles.Count() == 0)
                {
                    atleastOneFileFound = false;
                    log.Warn("No Files Found in Input Directory, Exiting Processing!");
                }
                else
                {
                    double divergencePercentage = Convert.ToDouble(ConfigurationManager.AppSettings["DivergencePercentage"]);
                    foreach (DataFile dataFile in dataFiles)
                    {
                        try
                        {
                            FileProcessStatus fileProcessStatus = FileProcessStatus.Undetermined;
                            fileProcessStatus = ProcessInputFile(dataFile, fileTypes, folderPath, divergencePercentage);
                            IArchivalHandler archivalHandler = new ArchivalHandler();
                            fileProcessStatus = ProcessFileArchival(fileProcessStatus, dataFile.FileName, folderPath, archivalHandler);
                        }
                        catch (Exception ex)
                        {
                            log.Error("File processing has unexpectedly failed for " + dataFile.FileName + " Continuing processing for other files, if any!");
                            log.Error(ex);
                        }

                    }
                    OutputStatistics(dataFiles, divergencePercentage);
                }


                log.Info("Read Completed");
                return atleastOneFileFound;
                
                
            }

            catch (Exception )
            {
                throw;
            }
        }
        
        public List<DataFile> FetchAllDataFiles(FileTypes fileTypes, string folderPath)
        {
            try
            {
                log.Debug("Start FetchAllDataFiles() ");
                List<DataFile> dataFiles = new List<DataFile>();
                //Loop for each file type configured
                foreach (FileType fileType in fileTypes.FileType)
                {
                    //Loop through each file and create a DataFile Object
                    foreach (string inputFile in Directory.GetFiles(folderPath, fileType.Identifier).ToList())
                    {
                        DataFile dataFile = new DataFile
                        {
                            FileName = inputFile.Substring(inputFile.LastIndexOf("\\"), (inputFile.Length - inputFile.LastIndexOf("\\"))),
                            FileType = fileType.Identifier
                        };
                        dataFiles.Add(dataFile);
                    }
                }

                
                return dataFiles;
            }

            catch(Exception)
            {
                throw;
            }
        }
        
        public FileProcessStatus ProcessInputFile(DataFile dataFile, FileTypes fileTypes, string folderPath, double divergencePercentage)
        {
            try
            {
                log.Debug("Start ProcessInputFile()");
                log.Info("Processing " + dataFile.FileName);

                #region Read data from each file and calculate median values

                    FileProcessStatus fileProcessStatus = FileProcessStatus.Undetermined;
                    FileType fileType = new FileType();
                    string[] file = ValidateInputFile(dataFile, fileTypes, ref fileType, folderPath, ref fileProcessStatus);

                if (fileProcessStatus == FileProcessStatus.FileValidationSucceeded)
                {
                    dataFile.MeterReads = ReadFile(file, fileType, ref fileProcessStatus);

                    if (fileProcessStatus == FileProcessStatus.FileReadSuccessfully)
                    {
                        double median = DataFile.CalculateMedian(dataFile.MeterReads);

                        foreach (FileRow meterRead in dataFile.MeterReads)
                        {
                            if (meterRead.DataValue > ((100 + divergencePercentage) * median / 100))
                                meterRead.Divergence = Divergence.MoreThanAcceptableDivergence;

                            else if (meterRead.DataValue < ((100 - divergencePercentage) * median / 100))
                                meterRead.Divergence = Divergence.LessThanAcceptableDivergence;

                            if ((meterRead.Divergence == Divergence.LessThanAcceptableDivergence) || (meterRead.Divergence == Divergence.MoreThanAcceptableDivergence))
                                Console.WriteLine(dataFile.FileName + " " + meterRead.OccurrenceDateTime.ToString() + " "
                                    + meterRead.DataValue.ToString()
                                    + " " + median.ToString());
                        }

                        dataFile.MedianValue = median;
                    }

                }
                else
                    return fileProcessStatus;

                #endregion

                if (fileProcessStatus != FileProcessStatus.FileRowsSkipped)
                    fileProcessStatus = FileProcessStatus.FileSuccessfullyProccessed;
                
                return fileProcessStatus;
            }

            catch (Exception)
            {
                throw;
            }
        }
        
        public string[] ValidateInputFile(DataFile dataFile, FileTypes fileTypes, ref FileType fileType, string folderPath, ref FileProcessStatus validationStatus)
        {
            try
            {
                log.Debug("Start ValidateInputFile()");
                //Check Blank files
                if (new FileInfo(folderPath + dataFile.FileName).Length == 0)
                {
                    log.Warn(dataFile.FileName + " is blank, nothing to read! ");

                    validationStatus = FileProcessStatus.BlankFileDetected;
                    return Array.Empty<string>();
                }

                else
                {
                    string[] csvLines = File.ReadAllLines(folderPath + dataFile.FileName);
                    string[] headers = csvLines[0].Split(',');

                    //Get the Column Names for the files and check if they contain the required column names, and check the indexes

                    fileType = fileTypes.FileType.FirstOrDefault(x => x.Identifier == dataFile.FileType);

                    foreach (Items item in fileType.Items)
                    {
                        item.Index = null;
                        for (int i = 0; i < headers.Count(); i++)
                        {
                            if (headers[i] == item.FileHeaderName)
                            {
                                item.Index = i;
                                break;
                            }
                        }

                        if (item.Index == null)
                        {
                            log.Error("Column " + item.FileHeaderName + " is missing in file: " + dataFile.FileName);
                            validationStatus = FileProcessStatus.FileHeadersNotFound;
                            return Array.Empty<string>();
                        }
                    }

                    validationStatus = FileProcessStatus.FileValidationSucceeded;

                    return csvLines;
                }
            }
            catch(Exception)
            {
                throw;
            }
        }
       
        public List<FileRow> ReadFile(string[] csvLines, FileType fileType,ref FileProcessStatus readFileStatus)
        {
            try
            {
                log.Debug("Start ReadFile()");
                List<FileRow> meterReads = new List<FileRow>();

                int dateTimeColumnIndex = Convert.ToInt32(fileType.Items.FirstOrDefault(x => x.ValueType == "Occurence DateTime").Index);
                int valueColumnIndex = Convert.ToInt32(fileType.Items.FirstOrDefault(x => x.ValueType == "Data Value").Index);

                foreach (string csvLine in csvLines.Skip(1))
                {

                    string[] values = csvLine.Split(',');


                    // Validation to check if the data is in the appropriate format                   

                    if (DateTime.TryParse(values[dateTimeColumnIndex], out DateTime tempDateTime))
                        tempDateTime = Convert.ToDateTime(values[dateTimeColumnIndex]);
                    else
                    {
                        readFileStatus = FileProcessStatus.FileRowsSkipped;
                        continue;
                    }

                    if (Double.TryParse(values[valueColumnIndex], out Double tempDouble))
                        tempDouble = Convert.ToDouble(values[valueColumnIndex]);
                    else
                    {
                        readFileStatus = FileProcessStatus.FileRowsSkipped;
                        continue;
                    }

                    FileRow meterRead = new FileRow(tempDateTime, tempDouble);
                    meterReads.Add(meterRead);

                }

                if (readFileStatus != FileProcessStatus.FileRowsSkipped)
                {
                    if (csvLines.Count() - 1 == meterReads.Count())
                        readFileStatus = FileProcessStatus.FileReadSuccessfully;
                }

                return meterReads;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public FileProcessStatus ArchiveFile(string fileName, string folderPath, FileArchivalType fileArchivalType, IArchivalHandler archivalHandler)
        {
            try
            {
                log.Debug("Start ArchiveFile()");
                string destinationFolderType;
                FileProcessStatus fileProcessStatus = FileProcessStatus.Undetermined;

                if (fileArchivalType == FileArchivalType.Archive)
                    destinationFolderType = "Archive";
                else if (fileArchivalType == FileArchivalType.Error)
                    destinationFolderType = "Error";
                else if (fileArchivalType == FileArchivalType.PartiallyProccessed)
                    destinationFolderType = "PartiallyProcessed";
                else
                    destinationFolderType = "Archive";


                string destinationFullPath = folderPath + "\\"+ destinationFolderType +"\\" + fileName;
                string sourceFullPath = folderPath + fileName;
               
                if (archivalHandler.CreateDirectory(folderPath + "\\" + destinationFolderType))
                {
                    if (!archivalHandler.FileExists(destinationFullPath))
                        fileProcessStatus = archivalHandler.MoveFile(sourceFullPath, destinationFullPath);
                    else
                    {
                        if (archivalHandler.DeleteFile(destinationFullPath))
                         fileProcessStatus = archivalHandler.MoveFile(sourceFullPath, destinationFullPath);
                    }
                }
                
                return fileProcessStatus;
            }

            catch (Exception)
            {
                throw;
            }

        }

       


        public FileProcessStatus ProcessFileArchival(FileProcessStatus fileProcessStatus, string fileName, string folderPath, IArchivalHandler archivalHandler)
        {
            try
            {
                log.Debug("Start ProcessFileArchival()");
                FileProcessStatus archiveProcessStatus = FileProcessStatus.Undetermined;
                if (fileProcessStatus == FileProcessStatus.FileSuccessfullyProccessed)
                {
                    archiveProcessStatus = ArchiveFile(fileName, folderPath, FileArchivalType.Archive, archivalHandler);
                    archiveProcessStatus = FileProcessStatus.FileSuccessfullyArchived;
                }
                else if ((fileProcessStatus == FileProcessStatus.FileHeadersNotFound) ||
                    (fileProcessStatus == FileProcessStatus.BlankFileDetected))
                {
                    archiveProcessStatus = ArchiveFile(fileName, folderPath, FileArchivalType.Error, archivalHandler);
                    archiveProcessStatus = FileProcessStatus.FileSuccessfullyArchivedToError;
                }
                else if (fileProcessStatus == FileProcessStatus.FileRowsSkipped)
                {
                    archiveProcessStatus = ArchiveFile(fileName, folderPath, FileArchivalType.PartiallyProccessed, archivalHandler);
                    archiveProcessStatus = FileProcessStatus.FileSuccessfullyArchivedToPartial;
                }
               


                return archiveProcessStatus;
            }
            catch(Exception)
            {
                throw;
            }

        }
        
        public void OutputStatistics(List<DataFile> datafiles, double divergencePercentage)
        {
            try
            {
                log.Debug("Start OutputStatistics()");
                log.Info("------------Processing Statistics--------------");

                foreach (DataFile dataFile in datafiles)
                {
                    log.Info("------------File Processed Name: " + dataFile.FileName + " ----------------------");

                    if (dataFile.MeterReads == null)
                    {
                        log.Info("Processing Failed for this file, please check Error Folder!");
                    }
                    else
                    {
                        log.Info("Total Number of Rows Processed: " + dataFile.MeterReads.Count().ToString());
                        log.Info("Median Value for file: " + dataFile.MedianValue.ToString());
                        log.Info("Number of Rows divergent from median by more than " + divergencePercentage.ToString() + " percent :" +
                            dataFile.MeterReads.FindAll(x => x.Divergence == Divergence.MoreThanAcceptableDivergence).Count().ToString());
                        log.Info("Number of Rows divergent from median by less than " + divergencePercentage.ToString() + " percent :" +
                            dataFile.MeterReads.FindAll(x => x.Divergence == Divergence.LessThanAcceptableDivergence).Count().ToString());

                    }
                    log.Info("-------------------------------------------------------------------------------------------------");
                }
            }

            catch (Exception)
            {
                throw;
            }

        }
    }
}
