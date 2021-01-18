using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace MCBTest.Models
{
    public class Customer
    {
        [XmlElement("customer_id")]
        public string Customer_Id { get; set; }
        public string Customer_Type { get; set; }
        public string Date_Of_Birth { get; set; }
        public string Date_Incorp { get; set; }
        public string Registration_No { get; set; }
        [XmlElement("Mailing_Address")]
        public Mailing Mailing_Address { get; set; }
        [XmlElement("Contact_Details")]
        public Contact Contact_Details { get; set; }
        [XmlElement("Shares_Details")]
        public Share Share_Details { get; set; }
    }
}