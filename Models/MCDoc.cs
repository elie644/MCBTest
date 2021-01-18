using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace MCBTest.Models
{
    [XmlRoot("RequestDoc")]
    public class MCDoc
    {
        public string Doc_Date { get; set; }
        public string Doc_Ref { get; set; }
        [XmlElement("Doc_Data")]
        public MCData Doc_Data { get; set; }
    }
}