using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace MCBTest.Models
{
    public class MCData
    {
        [XmlElement("DataItem_Customer")]
        public List<Customer> DataItem_Customer { get; set; }
    }
}