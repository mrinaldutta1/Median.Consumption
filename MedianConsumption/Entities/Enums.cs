using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{   
        public enum Divergence
        {
            MoreThanAcceptableDivergence = 1,
            LessThanAcceptableDivergence = 2,
            AcceptableDivergence=3

        }        

       public enum FileProcessStatus
        {
            Undetermined,
            BlankFileDetected,
            FileRowsSkipped,
            FileHeadersNotFound,
            FileValidationSucceeded,
            FileReadSuccessfully,
            FileSuccessfullyProccessed,
            FileSuccessfullyMoved,
            FileSuccessfullyArchived,
            FileSuccessfullyArchivedToError,
            FileSuccessfullyArchivedToPartial
        }

       public enum FileArchivalType
        {
           Archive,
           Error,
           PartiallyProccessed
        }
    
}
