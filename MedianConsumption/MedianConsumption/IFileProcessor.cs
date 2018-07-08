using System.Collections.Generic;
using Entities;

namespace MedianConsumption
{
    public interface IFileProcessor
    {
        /// <summary>
        /// Processes all files in the input directory
        /// </summary>
        bool ProcessAllFiles(string folderPath);

        /// <summary>
        /// Fetches All Data Files from the input directory
        /// </summary>
        /// <param name="fileTypes"></param>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        List<DataFile> FetchAllDataFiles(FileTypes fileTypes, string folderPath);

        /// <summary>
        /// Processes a  data file
        /// </summary>
        /// <param name="dataFiles"></param>
        /// <param name="fileTypes"></param>
        /// <param name="folderPath"></param>
        /// <param name="divergencePercentage"></param>
        /// <returns></returns>
        FileProcessStatus ProcessInputFile(DataFile dataFile, FileTypes fileTypes, string folderPath, double divergencePercentage);

        /// <summary>
        /// Validates an input file
        /// </summary>
        /// <param name="dataFile"></param>
        /// <param name="fileTypes"></param>
        /// <param name="fileType"></param>
        /// <param name="folderPath"></param>
        /// <param name="ValidationSuccess"></param>
        /// <returns></returns>
        string[] ValidateInputFile(DataFile dataFile, FileTypes fileTypes, ref FileType fileType, string folderPath, ref FileProcessStatus validationStatus);

        /// <summary>
        /// Reads csvlines and based on the filetype converts it into a list of rows
        /// </summary>
        /// <param name="csvLines"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        List<FileRow> ReadFile(string[] csvLines, FileType fileType, ref FileProcessStatus fileProcessStatus);

        /// <summary>
        /// Archives a file to a specified directory
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="folderPath"></param>
        FileProcessStatus ArchiveFile(string fileName, string folderPath, FileArchivalType fileArchivalType,IArchivalHandler archivalHandler);

        

        /// <summary>
        /// Decide on how files should be archived
        /// </summary>
        /// <param name="fileProcessStatus"></param>
        /// <param name="fileName"></param>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        FileProcessStatus ProcessFileArchival(FileProcessStatus fileProcessStatus, string fileName, string folderPath, IArchivalHandler archivalHandler);

        /// <summary>
        /// Prints Statistics at the end of processing
        /// </summary>
        /// <param name="datafiles"></param>
        void OutputStatistics(List<DataFile> datafiles, double divergencePercentage);
    }
}
;