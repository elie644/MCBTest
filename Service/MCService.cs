using MCBTest.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace MCBTest.Service
{
    public class MCService
    {
        private string config;

        public MCService()
        {
            config = ConfigurationManager.AppSettings["Connection"];
        }
        public List<Customer> ListCustomerIndividual()
        {
            List<Customer> lst = new List<Customer>();
            try
            {
                using (var dbConnection = new SqlConnection(config))
                {
                    dbConnection.Open();
                    var cmd = dbConnection.CreateCommand();
                    string sql = "SELECT * FROM Customers WHERE Type = 'Individual';";
                    cmd.CommandText = sql;
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Customer cli = new Customer();
                            cli.Customer_Id = rdr.GetString(0);
                            cli.Customer_Type = rdr.GetString(1);
                            if (!(rdr[2] is DBNull)) cli.Date_Of_Birth = rdr.GetDateTime(2).ToString();
                            if (!(rdr[3] is DBNull)) cli.Date_Incorp = rdr.GetDateTime(3).ToString();
                            cli.Registration_No = rdr[4] is DBNull ? "" : rdr.GetString(4);
                            cli.Mailing_Address = new Mailing();
                            cli.Mailing_Address.Address_Line1 = rdr.GetString(5);
                            cli.Mailing_Address.Address_Line2 = rdr.GetString(6);
                            cli.Mailing_Address.Town_City = rdr.GetString(7);
                            cli.Mailing_Address.Country = rdr.GetString(8);
                            cli.Contact_Details = new Contact();
                            cli.Contact_Details.Contact_Name = rdr.GetString(9);
                            cli.Contact_Details.Contact_Number = rdr.GetString(10);
                            cli.Share_Details = new Share();
                            cli.Share_Details.Num_Shares = rdr.GetInt32(11);
                            cli.Share_Details.Share_Price = rdr.GetDecimal(12);
                            cli.Share_Details.balace = rdr.GetDecimal(13);
                            lst.Add(cli);
                        }
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Verify your data!");
            }
            return lst;
        }
        public List<MCElement> ListCustomer()
        {
            List<MCElement> lst = new List<MCElement>();
            try
            {
                using (var dbConnection = new SqlConnection(config))
                {
                    dbConnection.Open();
                    var cmd = dbConnection.CreateCommand();
                    string sql = "SELECT Id, ContactName, DateOfBirth, DateIncorp, Type, NumShare, SharePrice, Balance FROM Customers;";
                    cmd.CommandText = sql;
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            MCElement cli = new MCElement();
                            cli.Id = rdr.GetString(0);
                            cli.ContactName = rdr.GetString(1);
                            cli.DateBirthOrIncorp = rdr[2] is DBNull ? rdr.GetDateTime(3) : rdr.GetDateTime(2);
                            cli.Type = rdr.GetString(4);
                            cli.NumShare = rdr.GetInt32(5);
                            cli.SharePrice = rdr.GetDecimal(6);
                            cli.Balance = rdr.GetDecimal(7);
                            lst.Add(cli);
                        }
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Date of Birth or Date Incorp is Null!");
            }
            return lst;
        }
        public List<Customer> ListCustomerSearch(string name)
        {
            List<Customer> lst = new List<Customer>();
            try
            {
                using (var dbConnection = new SqlConnection(config))
                {
                    dbConnection.Open();
                    var cmd = dbConnection.CreateCommand();
                    string sql = "SELECT * FROM Customers WHERE ContactName LIKE '%" + name + "%';";
                    cmd.CommandText = sql;
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Customer cli = new Customer();
                            cli.Customer_Id = rdr.GetString(0);
                            cli.Customer_Type = rdr.GetString(1);
                            if (!(rdr[2] is DBNull)) cli.Date_Of_Birth = rdr.GetDateTime(2).ToString();
                            if (!(rdr[3] is DBNull)) cli.Date_Incorp = rdr.GetDateTime(3).ToString();
                            cli.Registration_No = rdr[4] is DBNull ? "" : rdr.GetString(4);
                            cli.Mailing_Address = new Mailing();
                            cli.Mailing_Address.Address_Line1 = rdr.GetString(5);
                            cli.Mailing_Address.Address_Line2 = rdr.GetString(6);
                            cli.Mailing_Address.Town_City = rdr.GetString(7);
                            cli.Mailing_Address.Country = rdr.GetString(8);
                            cli.Contact_Details = new Contact();
                            cli.Contact_Details.Contact_Name = rdr.GetString(9);
                            cli.Contact_Details.Contact_Number = rdr.GetString(10);
                            cli.Share_Details = new Share();
                            cli.Share_Details.Num_Shares = rdr.GetInt32(11);
                            cli.Share_Details.Share_Price = rdr.GetDecimal(12);
                            cli.Share_Details.balace = rdr.GetDecimal(13);
                            lst.Add(cli);
                        }
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Verify your data!");
            }
            return lst;
        }
        public bool CheckDoc(string DocRef)
        {
            var b = false;
            using (var dbConnection = new SqlConnection(config))
            {
                dbConnection.Open();
                var cmd = dbConnection.CreateCommand();
                string sql = "SELECT TOP 1 DocRef FROM Customers WHERE DocRef = '" + DocRef + "';";
                cmd.CommandText = sql;
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    b = rdr.Read();
                }
                cmd.Dispose();
            }
            return b;
        }
        public bool InsertCustomer(MCDoc doc)
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-GB", false);
            var response = false;
            try
            {
                if (doc.Doc_Data.DataItem_Customer.Count != 0)
                {
                    using (var dbConnection = new SqlConnection(config))
                    {
                        SqlTransaction T;
                        dbConnection.Open();
                        T = dbConnection.BeginTransaction();
                        var cmd = dbConnection.CreateCommand();
                        cmd.Transaction = T;
                        try
                        {
                            foreach (Customer customer in doc.Doc_Data.DataItem_Customer)
                            {
                                var dob = !string.IsNullOrEmpty((customer.Date_Of_Birth)) ? "DateOfBirth, " : "";
                                var dob2 = !string.IsNullOrEmpty((customer.Date_Of_Birth)) ? $"'{customer.Date_Of_Birth}'," : "";
                                var di = !string.IsNullOrEmpty((customer.Date_Incorp)) ? "DateIncorp, " : "";
                                var di2 = !string.IsNullOrEmpty((customer.Date_Incorp)) ? $"'{customer.Date_Incorp}', " : "";
                                string sql = $"{$"INSERT INTO Customers (Id,Type, "}{dob}{di}RegistrationNO,AddressLine1,AddressLine2,TownCity,Country,ContactName,ContactNumber,NumShare,SharePrice,Balance,DocRef,DocDate) VALUES ('{customer.Customer_Id}', '{customer.Customer_Type}', {dob2} {di2}'{customer.Registration_No}', '{customer.Mailing_Address.Address_Line1}', '{customer.Mailing_Address.Address_Line2}', '{customer.Mailing_Address.Town_City}', '{customer.Mailing_Address.Country}', '{customer.Contact_Details.Contact_Name}', '{customer.Contact_Details.Contact_Number}', '{customer.Share_Details.Num_Shares}', '{customer.Share_Details.Share_Price}', '{customer.Share_Details.CalculBalance()}', '{doc.Doc_Ref}', '{doc.Doc_Date}');";
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                            }
                            T.Commit();
                        }
                        catch (Exception ex)
                        { 
                            T.Rollback();
                            response = false;
                        }
                        finally
                        {
                            T?.Dispose();
                            cmd.Dispose();
                            response = true;
                        }                     
                        
                    }
                }
                else response = false;
            }
            catch (Exception ex)
            {
                response = false;
            }
            return response;
        }
        public Customer GetbyId(string Id)
        {
            Customer cli = new Customer();
            try
            {
                using (var dbConnection = new SqlConnection(config))
                {
                    dbConnection.Open();
                    var cmd = dbConnection.CreateCommand();
                    string sql = "SELECT * FROM Customers WHERE Id = '" + Id + "';";
                    cmd.CommandText = sql;
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            cli.Customer_Id = rdr.GetString(0);
                            cli.Customer_Type = rdr.GetString(1);
                            if (!(rdr[2] is DBNull)) cli.Date_Of_Birth = rdr.GetDateTime(2).ToString();
                            if (!(rdr[3] is DBNull)) cli.Date_Incorp = rdr.GetDateTime(3).ToString();
                            cli.Registration_No = rdr[4] is DBNull ? "" : rdr.GetString(4);
                            cli.Mailing_Address = new Mailing();
                            cli.Mailing_Address.Address_Line1 = rdr.GetString(5);
                            cli.Mailing_Address.Address_Line2 = rdr.GetString(6);
                            cli.Mailing_Address.Town_City = rdr.GetString(7);
                            cli.Mailing_Address.Country = rdr.GetString(8);
                            cli.Contact_Details = new Contact();
                            cli.Contact_Details.Contact_Name = rdr.GetString(9);
                            cli.Contact_Details.Contact_Number = rdr.GetString(10);
                            cli.Share_Details = new Share();
                            cli.Share_Details.Num_Shares = rdr.GetInt32(11);
                            cli.Share_Details.Share_Price = rdr.GetDecimal(12);
                            cli.Share_Details.balace = rdr.GetDecimal(13);
                        }
                    }
                    cmd.Dispose();
                }
                if (cli.Customer_Id is null) throw new Exception("Verify your data!");
                else return cli;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool UpdateCustomer(Customer clt)
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-GB", false);
            var response = false;   
            try
            {
                if (clt.Customer_Id != "")
                {
                    Customer share = GetbyId(clt.Customer_Id);
                    int ns = share.Share_Details.Num_Shares;
                    share.Date_Of_Birth = clt.Date_Of_Birth;
                    share.Date_Incorp = clt.Date_Incorp;
                    share.Registration_No = clt.Registration_No;
                    share.Mailing_Address = new Mailing();
                    share.Mailing_Address.Address_Line1 = clt.Mailing_Address?.Address_Line1;
                    share.Mailing_Address.Address_Line2 = clt.Mailing_Address?.Address_Line2;
                    share.Mailing_Address.Town_City = clt.Mailing_Address?.Town_City;
                    share.Mailing_Address.Country = clt.Mailing_Address?.Country;
                    share.Contact_Details = new Contact();          
                    share.Contact_Details.Contact_Name = clt.Contact_Details?.Contact_Name;
                    share.Contact_Details.Contact_Number = clt.Contact_Details?.Contact_Number;
                    share.Share_Details = new Share();
                    share.Share_Details.Share_Price = clt.Share_Details.Share_Price;
                    share.Share_Details.Num_Shares = clt.Customer_Type == "Corporate" ? ns : clt.Share_Details.Num_Shares ;
                   
                    using (var dbConnection = new SqlConnection(config))
                    {
                        dbConnection.Open();
                        var cmd = dbConnection.CreateCommand();
                        var birth = share.Date_Of_Birth != null ? $"'{share.Date_Of_Birth}'" : "NULL";
                        var incorp = share.Date_Incorp != null ? $"'{share.Date_Incorp}'" : "NULL";
                        string sql = "UPDATE Customers SET Type = '" + share.Customer_Type + "'," +
                                     "DateOfBirth = "+ birth +"," +
                                     "DateIncorp = " + incorp + "," +
                                     "RegistrationNO = '" + share.Registration_No + "'," +
                                     "AddressLine1 = '" + share.Mailing_Address.Address_Line1 + "'," +
                                     "AddressLine2 = '" + share.Mailing_Address.Address_Line2 + "'," +
                                     "TownCity = '" + share.Mailing_Address.Town_City + "'," +
                                     "Country = '" + share.Mailing_Address.Country + "'," +
                                     "ContactName = '" + share.Contact_Details.Contact_Name + "'," +
                                     "ContactNumber = '" + share.Contact_Details.Contact_Number + "'," +
                                     "NumShare = '" + share.Share_Details.Num_Shares + "'," +
                                     "SharePrice = '" + share.Share_Details.Share_Price + "'," +
                                     "Balance = '" + share.Share_Details.CalculBalance() + "'" +
                                     "WHERE Id = '" + clt.Customer_Id + "';";
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        response = true;
                    }
                }
                else response = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
    }
}