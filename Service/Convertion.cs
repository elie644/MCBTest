using MCBTest.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace MCBTest.Service
{
    public class Convertion
    {
        public MCDoc GetAllCustomers(string filename)
        {
            try
            {
                if (string.IsNullOrEmpty(filename))
                    throw new Exception("Verify your filename!");

                var serializer = new XmlSerializer(typeof(MCDoc));
                using (var fs = new FileStream(filename, FileMode.Open))
                {
                    var cstm = (MCDoc)serializer.Deserialize(fs);
                    return cstm;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool Old(string dt)
        {
            bool bl = false;
            DateTime dtt;
            if (dt != "")
            {
                var isparsed = DateTime.TryParse(dt, out dtt);
                if(isparsed)
                {
                    var years = DateTime.Now.Year - dtt.Year;
                    return years >= 18;
                }
                throw new Exception("Format date invalid!");
            }
            else return true;
        }
        public bool TestShare(int num, decimal pri)
        {
            bool bl = false;
            decimal prir = decimal.Round(pri, 2);
            try
            {
                return ((num > 0) && (pri > 0)) && (prir.Equals(pri));
            }
            catch (Exception ex)
            {
                return bl;
            }
        }
        public int Verification(MCData doc)
        {
            int code = 000;
            try
            {
                if (doc.DataItem_Customer.Count !=0)
                {
                    foreach (Customer customer in doc.DataItem_Customer)
                    {
                        if(customer.Customer_Type=="Individual")
                        {
                            if (Old(customer.Date_Of_Birth))
                            {
                                code = TestShare(customer.Share_Details.Num_Shares, customer.Share_Details.Share_Price) ? 111 : 532;
                                if (code == 532) return code;
                            }
                            else
                                return code = 531;
                        }
                        else
                        {
                            code = TestShare(customer.Share_Details.Num_Shares, customer.Share_Details.Share_Price) ? 111 : 532;
                            if (code == 532) return code;
                        }
                    }
                }
                return code;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool Csv(string file, List<MCElement> Customer)
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-GB", false);
            bool rslt = false;
            try
            {
                if (!String.IsNullOrEmpty(file))
                {
                    string date = DateTime.Now.ToShortDateString().ToString().Replace('/', '_');
                    string filename = Path.Combine(file, "MyTest" + date + ".csv");
                    var sb = new StringBuilder();
                    for (int i = 0; i < Customer.Count; i++)
                    {
                        sb.Append(Customer[i].Id + ',' + Customer[i].ContactName + ',' + Customer[i].DateBirthOrIncorp + ','
                                + Customer[i].Type + ',' + Customer[i].NumShare + ','
                                + Customer[i].SharePrice + ',' + Customer[i].Balance + ';');
                        sb.Append("\r\n");
                    }

                    File.WriteAllText(filename, sb.ToString());
                    rslt = true;
                    return rslt;
                }
                else throw new Exception("Verify your filename!");
            }
            catch(Exception ex)
            {
                throw ex;
            }         
        }
    }
}