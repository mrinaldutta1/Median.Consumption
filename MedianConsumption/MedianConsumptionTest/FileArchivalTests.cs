using System;
using Entities;
using System.IO;
using System.Linq;
using MedianConsumption;
using NUnit.Framework;
using Moq;

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
    }
}
