using System;
using Entities;
using System.IO;
using System.Linq;
using MedianConsumption;
using NUnit.Framework;
using Moq;
using System.Collections.Generic;

namespace MedianConsumptionTest
{
    [TestFixture]
    class FileArchivalTests
    {
        [Test]
        public void TestFileArchivalForArchiveFolder()
        {
            Mock<IArchivalHandler> mockArchivalHandler = new Mock<IArchivalHandler>();
            mockArchivalHandler.Setup(x => x.CreateDirectory(It.IsAny<string>())).Returns(true);
            mockArchivalHandler.Setup(x => x.MoveFile(It.IsAny<string>(), It.IsAny<string>())).Returns(FileProcessStatus.FileSuccessfullyMoved);
            FileProcessor fileProcessor = new FileProcessor();
            FileProcessStatus fileProcessStatus = fileProcessor.ArchiveFile(It.IsAny<string>(), It.IsAny<string>(), FileArchivalType.Archive, mockArchivalHandler.Object);
            Assert.AreEqual(FileProcessStatus.FileSuccessfullyMoved, fileProcessStatus);

        }


        [Test]
        public void TestFileArchivalForErrorFolder()
        {
            Mock<IArchivalHandler> mockArchivalHandler = new Mock<IArchivalHandler>();
            mockArchivalHandler.Setup(x => x.CreateDirectory(It.IsAny<string>())).Returns(true);
            mockArchivalHandler.Setup(x => x.MoveFile(It.IsAny<string>(), It.IsAny<string>())).Returns(FileProcessStatus.FileSuccessfullyMoved);
            FileProcessor fileProcessor = new FileProcessor();
            FileProcessStatus fileProcessStatus = fileProcessor.ArchiveFile(It.IsAny<string>(), It.IsAny<string>(), FileArchivalType.Error, mockArchivalHandler.Object);
            Assert.AreEqual(FileProcessStatus.FileSuccessfullyMoved, fileProcessStatus);

        }

        [Test]
        public void TestFileArchivalForPartialFolder()
        {
            Mock<IArchivalHandler> mockArchivalHandler = new Mock<IArchivalHandler>();
            mockArchivalHandler.Setup(x => x.CreateDirectory(It.IsAny<string>())).Returns(true);
            mockArchivalHandler.Setup(x => x.MoveFile(It.IsAny<string>(), It.IsAny<string>())).Returns(FileProcessStatus.FileSuccessfullyMoved);
            FileProcessor fileProcessor = new FileProcessor();
            FileProcessStatus fileProcessStatus = fileProcessor.ArchiveFile(It.IsAny<string>(), It.IsAny<string>(), FileArchivalType.PartiallyProccessed, mockArchivalHandler.Object);
            Assert.AreEqual(FileProcessStatus.FileSuccessfullyMoved, fileProcessStatus);

        }

        [Test]
        public void TestFileExistsinArchivalFolder()
        {
            Mock<IArchivalHandler> mockArchivalHandler = new Mock<IArchivalHandler>();
            mockArchivalHandler.Setup(x => x.CreateDirectory(It.IsAny<string>())).Returns(true);
            mockArchivalHandler.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
            mockArchivalHandler.Setup(x => x.DeleteFile(It.IsAny<string>())).Returns(true);
            mockArchivalHandler.Setup(x => x.MoveFile(It.IsAny<string>(), It.IsAny<string>())).Returns(FileProcessStatus.FileSuccessfullyMoved);
            FileProcessor fileProcessor = new FileProcessor();
            FileProcessStatus fileProcessStatus = fileProcessor.ArchiveFile(It.IsAny<string>(), It.IsAny<string>(), FileArchivalType.PartiallyProccessed, mockArchivalHandler.Object);
            Assert.AreEqual(FileProcessStatus.FileSuccessfullyMoved, fileProcessStatus);

        }

        [Test]
        public void TestFileArchivalSuccessfullyProcessed()
        {
            Mock<IArchivalHandler> mockArchivalHandler = new Mock<IArchivalHandler>();
            mockArchivalHandler.Setup(x => x.CreateDirectory(It.IsAny<string>())).Returns(true);
            mockArchivalHandler.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
            mockArchivalHandler.Setup(x => x.DeleteFile(It.IsAny<string>())).Returns(true);
            mockArchivalHandler.Setup(x => x.MoveFile(It.IsAny<string>(), It.IsAny<string>())).Returns(FileProcessStatus.FileSuccessfullyArchived);
            FileProcessor fileProcessor = new FileProcessor();
            FileProcessStatus fileProcessStatus = FileProcessStatus.FileSuccessfullyProccessed;
            fileProcessStatus = fileProcessor.ProcessFileArchival(fileProcessStatus, "Foobar.csv", "Foobar/Foobar", mockArchivalHandler.Object);
            Assert.AreEqual(FileProcessStatus.FileSuccessfullyArchived , fileProcessStatus);
        }

        [Test]
        public void TestFileArchivalHeaderNotFound()
        {
            Mock<IArchivalHandler> mockArchivalHandler = new Mock<IArchivalHandler>();
            mockArchivalHandler.Setup(x => x.CreateDirectory(It.IsAny<string>())).Returns(true);
            mockArchivalHandler.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
            mockArchivalHandler.Setup(x => x.DeleteFile(It.IsAny<string>())).Returns(true);
            mockArchivalHandler.Setup(x => x.MoveFile(It.IsAny<string>(), It.IsAny<string>())).Returns(FileProcessStatus.FileSuccessfullyArchived);
            FileProcessor fileProcessor = new FileProcessor();
            FileProcessStatus fileProcessStatus = FileProcessStatus.FileHeadersNotFound;
            fileProcessStatus = fileProcessor.ProcessFileArchival(fileProcessStatus, "Foobar.csv", "Foobar/Foobar", mockArchivalHandler.Object);
            Assert.AreEqual(FileProcessStatus.FileSuccessfullyArchivedToError, fileProcessStatus);
        }

        [Test]
        public void TestFileArchivalBlankFileDetected()
        {
            Mock<IArchivalHandler> mockArchivalHandler = new Mock<IArchivalHandler>();
            mockArchivalHandler.Setup(x => x.CreateDirectory(It.IsAny<string>())).Returns(true);
            mockArchivalHandler.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
            mockArchivalHandler.Setup(x => x.DeleteFile(It.IsAny<string>())).Returns(true);
            mockArchivalHandler.Setup(x => x.MoveFile(It.IsAny<string>(), It.IsAny<string>())).Returns(FileProcessStatus.FileSuccessfullyArchived);
            FileProcessor fileProcessor = new FileProcessor();
            FileProcessStatus fileProcessStatus = FileProcessStatus.BlankFileDetected;
            fileProcessStatus = fileProcessor.ProcessFileArchival(fileProcessStatus, "Foobar.csv", "Foobar/Foobar", mockArchivalHandler.Object);
            Assert.AreEqual(FileProcessStatus.FileSuccessfullyArchivedToError, fileProcessStatus);
        }

        [Test]
        public void TestFileArchivalFileRowsSkipped()
        {
            Mock<IArchivalHandler> mockArchivalHandler = new Mock<IArchivalHandler>();
            mockArchivalHandler.Setup(x => x.CreateDirectory(It.IsAny<string>())).Returns(true);
            mockArchivalHandler.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
            mockArchivalHandler.Setup(x => x.DeleteFile(It.IsAny<string>())).Returns(true);
            mockArchivalHandler.Setup(x => x.MoveFile(It.IsAny<string>(), It.IsAny<string>())).Returns(FileProcessStatus.FileSuccessfullyArchived);
            FileProcessor fileProcessor = new FileProcessor();
            FileProcessStatus fileProcessStatus = FileProcessStatus.FileRowsSkipped ;
            fileProcessStatus = fileProcessor.ProcessFileArchival(fileProcessStatus, "Foobar.csv", "Foobar/Foobar", mockArchivalHandler.Object);
            Assert.AreEqual(FileProcessStatus.FileSuccessfullyArchivedToPartial , fileProcessStatus);
        }

        [TestCase() ]
        public void TestProcessAllFiles()
        {
            string startupPath = AppDomain.CurrentDomain.BaseDirectory;
            var pathItems = startupPath.Split(Path.DirectorySeparatorChar);
            var pos = pathItems.Reverse().ToList().FindIndex(x => string.Equals("bin", x));
            string projectPath = String.Join(Path.DirectorySeparatorChar.ToString(), pathItems.Take(pathItems.Length - pos - 1));
            string inputLocation = Path.Combine(projectPath, "UnitTestFiles","FolderToTestProcessAllFiles");

            Mock<IArchivalHandler> mockArchivalHandler = new Mock<IArchivalHandler>();
            mockArchivalHandler.Setup(x => x.CreateDirectory(It.IsAny<string>())).Returns(true);
            mockArchivalHandler.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
            mockArchivalHandler.Setup(x => x.DeleteFile(It.IsAny<string>())).Returns(true);
            mockArchivalHandler.Setup(x => x.MoveFile(It.IsAny<string>(), It.IsAny<string>())).Returns(FileProcessStatus.FileSuccessfullyArchived);

            List<DataFile> dataFiles =  new List<DataFile>();
            IFileProcessor fileProcessor =  new FileProcessor();
            fileProcessor.ProcessAllFiles(inputLocation, mockArchivalHandler.Object, ref dataFiles);
            Assert.AreEqual(FileProcessStatus.FileSuccessfullyArchived, dataFiles.Find(x => x.FileName.Contains("LP_ValidFile12Rows")).ProcessingStatus);
            Assert.AreEqual(FileProcessStatus.FileSuccessfullyArchived, dataFiles.Find(x => x.FileName.Contains("TOU_ValidFile9Rows")).ProcessingStatus);
            Assert.AreEqual(FileProcessStatus.FileSuccessfullyArchivedToError, dataFiles.Find(x => x.FileName.Contains("LP_BlankFile")).ProcessingStatus);
            Assert.AreEqual(FileProcessStatus.FileSuccessfullyArchivedToError, dataFiles.Find(x => x.FileName.Contains("LP_OnlyHeader")).ProcessingStatus);
            Assert.AreEqual(FileProcessStatus.FileSuccessfullyArchivedToPartial, dataFiles.Find(x => x.FileName.Contains("TOU_MissingData")).ProcessingStatus);


        }
    }
}
