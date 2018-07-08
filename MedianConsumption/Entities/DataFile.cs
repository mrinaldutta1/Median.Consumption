using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Entities
{


    /// <summary>
    /// Class For Input DataFiles
    /// </summary>
    public class DataFile
    {
        private readonly static log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public string FileType { get; set; }
        public string FileName { get; set; }
        public double MedianValue { get; set; }
        public List<FileRow> MeterReads { get; set; }
        public FileProcessStatus ProcessingStatus { get;set; }

       
        public DataFile()
        {

        }
        /// <summary>
        /// Calculates Median value based on an input list
        /// </summary>
        /// <param name="meterReads"></param>
        /// <returns></returns>
        public static double CalculateMedian(List<FileRow> meterReads)
        {
            try
            {
                log.Debug("Start CalculateMedian()");

                double median;

                if (meterReads.Count() == 1)
                    return meterReads[0].DataValue;

                List<FileRow> sortedMeterReads = meterReads.OrderBy(o => o.DataValue).ToList();
                int size = sortedMeterReads.Count();
                int mid = size / 2;
                if (size % 2 != 0)
                    median = sortedMeterReads[mid].DataValue;
                else
                    median = (sortedMeterReads[mid].DataValue +
                             sortedMeterReads[mid - 1].DataValue) / 2;

                return median;
            }

            catch (Exception)
            {
                throw;
            }


        }

       
    }

    public class FileRow
    {
        public DateTime OccurrenceDateTime { get; set; }
        public double DataValue { get; set; }
        public Divergence Divergence { get; set; }

        public FileRow(DateTime meterReadDateTime, double granularMeterRead)
        {
            this.OccurrenceDateTime = meterReadDateTime;
            this.DataValue = granularMeterRead;
            this.Divergence = Divergence.AcceptableDivergence;// Default Acceptance unless overridden
        }

        
       
    }



}
