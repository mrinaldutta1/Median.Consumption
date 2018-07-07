using System;
using System.Text;
using System.Collections.Generic;
using Entities;
using System.IO;
using System.Linq;
using MedianConsumption;
using NUnit.Framework;

namespace MedianConsumptionTest
{
   
    [TestFixture]
    public class ProcessingTests
    {
        public static object ApplicationEnvironment { get; private set; }

        /// <summary>
        /// Test the positive scenario of correct files
        /// </summary>
        [TestCase("LP_ValidFile12Rows",40, 3.8, 0, 0, 12)]
        [TestCase("LP_ValidFile12Rows",20, 3.8, 1, 1, 12)]
        [TestCase("LP_ValidFile12Rows",0, 3.8, 12, 12, 12)]
        [TestCase("TOU_ValidFile9Rows", 20, 0.146, 0, 2, 9)]
        [TestCase("TOU_ValidFile9Rows", 100000, 0.146, 0, 0, 9)]
        public void TestPositiveFilesProcessing(string fileName, double divergencePercentage, double medianResult, int noOfMoreDivergentResult, int noOfLessDivergentResult, int noOfLinesProcessedResult )
        {           

            string testFolder = GetTestDataFolder();
            IFileProcessor fileProcessor = new FileProcessor();
            FileTypes fileTypes=  FileTypes.GetFileTypes();
            DataFile testFile = fileProcessor.FetchAllDataFiles(fileTypes, testFolder)
                                              .Find(x => x.FileName.Contains(fileName));
            fileProcessor.ProcessInputFile(testFile, fileTypes, testFolder, divergencePercentage);

            //Tests Median Value
            Assert.AreEqual(medianResult, testFile.MedianValue);

            //Tests The Number of values more than divergence percentage
            Assert.AreEqual(noOfMoreDivergentResult, testFile.MeterReads.FindAll(x => x.Divergence == Divergence.MoreThanAcceptableDivergence).Count());

            //Tests the Number of values less than divergence percentage
            Assert.AreEqual(noOfLessDivergentResult, testFile.MeterReads.FindAll(x => x.Divergence == Divergence.LessThanAcceptableDivergence).Count());

            //Test the number of files processed
            Assert.AreEqual(noOfLinesProcessedResult, testFile.MeterReads.Count());

        }
        /// <summary>
        /// Tests if Blank Files are correctly not processed
        /// </summary>
        /// <param name="fileName"></param>
        [TestCase("BlankFile")]
        public void TestValidateBlankFile(string fileName)
        {
            string testFolder = GetTestDataFolder();
            IFileProcessor fileProcessor = new FileProcessor();
            FileTypes fileTypes = FileTypes.GetFileTypes();
            DataFile testFile = fileProcessor.FetchAllDataFiles(fileTypes, testFolder)
                                              .Find(x => x.FileName.Contains(fileName));
            FileType fileType = new FileType();
            FileProcessStatus validationSuccess = FileProcessStatus.Undetermined;
            string[] file = fileProcessor.ValidateInputFile(testFile, fileTypes, ref fileType, testFolder, ref validationSuccess);

            //Test if the array returned by the validate function has nothing in it
            Assert.AreEqual(0, file.Count());

            //Test if correct fileprocessing status is returned
            Assert.AreEqual(FileProcessStatus.BlankFileDetected, validationSuccess);
        }
        /// <summary>
        /// Tests the number of files matching the condition has been correctly picked up
        /// </summary>
        /// <param name="noOfFilesResult"></param>
        [TestCase(4)]
        public void TestNumberOfFilesProcessed(int noOfFilesResult)
        {
            string testFolder = GetTestDataFolder();
            IFileProcessor fileProcessor = new FileProcessor();
            FileTypes fileTypes = FileTypes.GetFileTypes();
            int fileCount = fileProcessor.FetchAllDataFiles(fileTypes, testFolder).Count();
            Assert.AreEqual(noOfFilesResult, fileCount);
        }

        [TestCase("TOU_MissingColumn")]
        public void TestMissingColumnFile(string fileName)
        {
            string testFolder = GetTestDataFolder();
            IFileProcessor fileProcessor = new FileProcessor();
            FileTypes fileTypes = FileTypes.GetFileTypes();
            DataFile testFile = fileProcessor.FetchAllDataFiles(fileTypes, testFolder)
                                              .Find(x => x.FileName.Contains(fileName));
            FileType fileType = new FileType();
            FileProcessStatus validationSuccess = FileProcessStatus.Undetermined;
            string[] file = fileProcessor.ValidateInputFile(testFile, fileTypes, ref fileType, testFolder, ref validationSuccess);
            Assert.AreEqual(validationSuccess, FileProcessStatus.FileHeadersNotFound);

        }

        [TestCase("TOU_MissingData")]
        public void TestMissingDataFile(string fileName)
        {
            string testFolder = GetTestDataFolder();
            IFileProcessor fileProcessor = new FileProcessor();
            FileTypes fileTypes = FileTypes.GetFileTypes();
            DataFile testFile = fileProcessor.FetchAllDataFiles(fileTypes, testFolder)
                                              .Find(x => x.FileName.Contains(fileName));
            FileType fileType = new FileType();
            FileProcessStatus validationSuccess = FileProcessStatus.Undetermined;
            string[] file = fileProcessor.ValidateInputFile(testFile, fileTypes, ref fileType, testFolder, ref validationSuccess);
            testFile.MeterReads = fileProcessor.ReadFile(file, fileType, ref validationSuccess);
            Assert.AreEqual(validationSuccess, FileProcessStatus.FileRowsSkipped);

        }


        public static string GetTestDataFolder()
        {
            string startupPath = AppDomain.CurrentDomain.BaseDirectory;
            var pathItems = startupPath.Split(Path.DirectorySeparatorChar);
            var pos = pathItems.Reverse().ToList().FindIndex(x => string.Equals("bin", x));
            string projectPath = String.Join(Path.DirectorySeparatorChar.ToString(), pathItems.Take(pathItems.Length - pos - 1));
            return Path.Combine(projectPath,"UnitTestFiles");
        }
    }
}
