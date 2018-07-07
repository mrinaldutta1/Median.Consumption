using System;
using NUnit.Framework;
using Entities;
using System.Collections;
using System.Collections.Generic;


namespace MedianConsumptionTest
{
    [TestFixture]
    public class MedianTests
    {
        [Test]
        public void TestMethodOddNumberOfRows()
        {
            FileRow fileRow1 = new FileRow(Convert.ToDateTime("2018-01-01 1:00"), 2);
            FileRow fileRow2 = new FileRow(Convert.ToDateTime("2018-01-01 1:00"), 9);
            FileRow fileRow3 = new FileRow(Convert.ToDateTime("2018-01-01 1:00"), .1);
            FileRow fileRow4 = new FileRow(Convert.ToDateTime("2018-01-01 1:00"), 89);
            FileRow fileRow5 = new FileRow(Convert.ToDateTime("2018-01-01 1:00"), -123);
            FileRow fileRow6 = new FileRow(Convert.ToDateTime("2018-01-01 1:00"), 56);
            FileRow fileRow7 = new FileRow(Convert.ToDateTime("2018-01-01 1:00"), 34);
            FileRow fileRow8 = new FileRow(Convert.ToDateTime("2018-01-01 1:00"), 10);
            FileRow fileRow9 = new FileRow(Convert.ToDateTime("2018-01-01 1:00"), 12);
            List<FileRow> files = new List<FileRow>
            {
                fileRow1,
                fileRow2,
                fileRow3,
                fileRow4,
                fileRow5,
                fileRow6,
                fileRow7,
                fileRow8,
                fileRow9
            };

            DataFile dataFile = new DataFile
            {
                MeterReads = files
            };

            double median = DataFile.CalculateMedian(files);
            Assert.AreEqual(10, median);
        }


        [Test]
        public void TestMethodEvenNumberOfData()
        {
            FileRow fileRow1 = new FileRow(Convert.ToDateTime("2018-01-01 1:00"), 2);
            FileRow fileRow2 = new FileRow(Convert.ToDateTime("2018-01-01 1:00"), 9);
            FileRow fileRow3 = new FileRow(Convert.ToDateTime("2018-01-01 1:00"), .1);
            FileRow fileRow4 = new FileRow(Convert.ToDateTime("2018-01-01 1:00"), 89);
            FileRow fileRow5 = new FileRow(Convert.ToDateTime("2018-01-01 1:00"), -123);
            FileRow fileRow6 = new FileRow(Convert.ToDateTime("2018-01-01 1:00"), 56);
            FileRow fileRow7 = new FileRow(Convert.ToDateTime("2018-01-01 1:00"), 34);
            FileRow fileRow8 = new FileRow(Convert.ToDateTime("2018-01-01 1:00"), 10);
            FileRow fileRow9 = new FileRow(Convert.ToDateTime("2018-01-01 1:00"), 12);
            FileRow fileRow10 = new FileRow(Convert.ToDateTime("2018-01-01 1:00"), 11);
            List<FileRow> files = new List<FileRow>
            {
                fileRow1,
                fileRow2,
                fileRow3,
                fileRow4,
                fileRow5,
                fileRow6,
                fileRow7,
                fileRow8,
                fileRow9,
                fileRow10
            };

            DataFile dataFile = new DataFile
            {
                MeterReads = files
            };

            double median = DataFile.CalculateMedian(files);
            Assert.AreEqual(10.5, median);
        }

        [Test]
        public void TestSingleRow()
        {
            FileRow fileRow1 = new FileRow(Convert.ToDateTime("2018-01-01 1:00"), 2);
            List<FileRow> files = new List<FileRow>
            {
                fileRow1
            };

            DataFile dataFile = new DataFile
            {
                MeterReads = files
            };
            double median = DataFile.CalculateMedian(files);
            Assert.AreEqual(2, median);
        }
    }
}
