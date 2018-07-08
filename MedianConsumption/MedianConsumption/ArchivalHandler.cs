using Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedianConsumption
{
    public class ArchivalHandler:IArchivalHandler
    {
        public bool CreateDirectory(string directory)
        {
            Directory.CreateDirectory(directory);
            return true;
        }

        public FileProcessStatus MoveFile(string sourceFullPath, string destinationFullPath)
        {
            File.Move(sourceFullPath, destinationFullPath);
            return FileProcessStatus.FileSuccessfullyMoved;
        }

        public bool DeleteFile(string destinationFullPath)
        {
            File.Delete(destinationFullPath);
            return true;
        }

        public bool FileExists(string destinationPath)
        {
            return File.Exists(destinationPath);
        }
    }
}
