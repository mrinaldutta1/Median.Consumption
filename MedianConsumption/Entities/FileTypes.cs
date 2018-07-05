using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Entities
{
   
        [XmlRoot(ElementName = "Item")]
        public class Item
        {
            [XmlElement(ElementName = "ValueType")]
            public string ValueType { get; set; }
            [XmlElement(ElementName = "FileHeaderName")]
            public string FileHeaderName { get; set; }
            [XmlElement(ElementName = "DotNetDataType")]
            public string DotNetDataType { get; set; }
            public int? Index { get; set; }
        }

        [XmlRoot(ElementName = "FileType")]
        public class FileType
        {
            [XmlElement(ElementName = "Identifier")]
            public string Identifier { get; set; }
            [XmlElement(ElementName = "Item")]
            public List<Item> Item { get; set; }
        }

        [XmlRoot(ElementName = "FileTypes")]
        public class FileTypes
        {
            [XmlElement(ElementName = "FileType")]
            public List<FileType> FileType { get; set; }
        }

    

}


