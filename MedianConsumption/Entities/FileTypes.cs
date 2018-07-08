using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace Entities
{

    [XmlRoot(ElementName = "Items")]
        public class Items
        {
            [XmlElement(ElementName = "ValueType")]
            public string ValueType { get; set; }
            [XmlElement(ElementName = "FileHeaderName")]
            public string FileHeaderName { get; set; }           
            public int? Index { get; set; }
        }

        [XmlRoot(ElementName = "FileType")]
        public class FileType
        {
            [XmlElement(ElementName = "Identifier")]
            public string Identifier { get; set; }
            [XmlElement(ElementName = "Items")]
            public List<Items> Items { get; set; }
        }

        [XmlRoot(ElementName = "FileTypes")]
        public class FileTypes
        {
            [XmlElement(ElementName = "FileType")]
            public List<FileType> FileType { get; set; }

            public static FileTypes GetFileTypes()
            {
                string fileTypeXMLpath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"FileConfiguration.xml");
                FileTypes fileTypes = new FileTypes();

                XmlSerializer serializer = new XmlSerializer(typeof(FileTypes));
                using (FileStream fileStream = new FileStream(fileTypeXMLpath, FileMode.Open))
                {
                    fileTypes = (FileTypes)serializer.Deserialize(fileStream);
                }

             return fileTypes;

            }
        }

    

}


