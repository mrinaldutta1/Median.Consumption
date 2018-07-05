using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Entities;
using System.Collections;
using System.Collections.Generic;


namespace MedianConsumptionTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethodOddNumberOfData()
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
            List<FileRow> files = new List<FileRow>();
            files.Add(fileRow1);
            files.Add(fileRow2);
            files.Add(fileRow3);
            files.Add(fileRow4);
            files.Add(fileRow5);
            files.Add(fileRow6);
            files.Add(fileRow7);
            files.Add(fileRow8);
            files.Add(fileRow9);

            DataFile dataFile = new DataFile();
            dataFile.MeterReads = files;

            double median = DataFile.CalculateMedian(files);
            Assert.AreEqual(10, median);
        }


        [TestMethod]
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
            List<FileRow> files = new List<FileRow>();
            files.Add(fileRow1);
            files.Add(fileRow2);
            files.Add(fileRow3);
            files.Add(fileRow4);
            files.Add(fileRow5);
            files.Add(fileRow6);
            files.Add(fileRow7);
            files.Add(fileRow8);
            files.Add(fileRow9);
            files.Add(fileRow10);

            DataFile dataFile = new DataFile();
            dataFile.MeterReads = files;

            double median = DataFile.CalculateMedian(files);
            Assert.AreEqual(10.5, median);
        }
    }
}
