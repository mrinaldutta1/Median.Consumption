using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedianConsumption
{
    public interface IArchivalHandler
    {
        bool CreateDirectory(string directory);

        FileProcessStatus MoveFile(string sourceFullPath, string destinationFullPath);

        bool DeleteFile(string destinationFullPath);

        bool FileExists(string destinationPath);
    }
}
