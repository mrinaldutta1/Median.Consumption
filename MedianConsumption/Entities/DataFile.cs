using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{


    public class DataFile
    {
        public string FileType { get; set; }
        public string FileName { get; set; }
        public double MedianValue { get; set; }
        public List<FileRow> MeterReads { get; set; }

        public DataFile(string fileName, double medianValue, List<FileRow> meterReads)
        {
            this.FileName = fileName;
            this.MedianValue = medianValue;
            this.MeterReads = meterReads;
        }

        public DataFile()
        {

        }

        public static double CalculateMedian(List<FileRow> meterReads)
        {
            try
            {

                double median;

                if (meterReads.Count() == 1)
                    return meterReads[0].DataValue;

                List<FileRow> sortedMeterReads = meterReads.OrderBy(o => o.DataValue).ToList();
                int size = sortedMeterReads.Count();
                int mid = size / 2;
                if (size % 2 == 0)
                    median = sortedMeterReads[mid].DataValue;
                else
                    median = (sortedMeterReads[mid].DataValue +
                             sortedMeterReads[mid + 1].DataValue) / 2;

                return median;
            }

            catch (Exception ex)
            {
                throw ex;
            }


        }

    }

    public class FileRow
    {
        public DateTime OccurenceDateTime { get; set; }
        public double DataValue { get; set; }
        public Enums.Divergence Divergence { get; set; }

        public FileRow(DateTime meterReadDateTime, double granularMeterRead)
        {
            this.OccurenceDateTime = meterReadDateTime;
            this.DataValue = granularMeterRead;
        }

        public FileRow()
        {

        }


        //Static Method to return a MeterRead object for LP and TOU kind of files

        public static FileRow FromCsv(string CsvLine)
        {
            string[] values = CsvLine.Split(',');
            FileRow meterRead = new FileRow
            {                
                OccurenceDateTime = Convert.ToDateTime(values[3]),
                DataValue = Convert.ToDouble(values[5])
            };
            return meterRead;
        }
    }



}
