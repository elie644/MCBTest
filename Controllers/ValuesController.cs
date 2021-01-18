using MCBTest.Service;
using MCBTest.Models;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Swashbuckle.Swagger.Annotations;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace MCBTest.Controllers
{
    [Produces("application/json")]
    [System.Web.Http.Route("api/[controller]")]
    [ApiController]
    public class ValuesController : Controller
    {
        MCService mC = new MCService();
        Convertion convertion = new Convertion();
        [System.Web.Mvc.HttpGet]
        public JsonResult GetSave(string file)
        {
            //file = Path.Combine(Server.MapPath("~"), "App_Data", "McShares_2018.xml");
            try
            {
                var customer = convertion.GetAllCustomers(file);
                if (mC.CheckDoc(customer.Doc_Ref))
                {
                    return Json(new { statut = "KO", message = "This document is already in the database" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    bool result;
                    var rst = convertion.Verification(customer.Doc_Data);
                    switch (rst)
                    {
                        case 111:
                            result = mC.InsertCustomer(customer);
                            return Json(new { statut = "OK", message = "Save with succes!", data = result }, JsonRequestBehavior.AllowGet);
                        case 531:
                            return Json(new { statut = "KO", message = "Customer must be at least 18 years old or verify date format", code = 531 }, JsonRequestBehavior.AllowGet);
                        case 532:
                            return Json(new { statut = "KO", message = "Values must be greater than 0 or verify a decimal place", code = 532 }, JsonRequestBehavior.AllowGet);
                        default:
                            return Json(new { statut = "KO", message = "" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { statut = "KO", message = ex.Message}, JsonRequestBehavior.AllowGet);
            }         
        } 

        [System.Web.Mvc.HttpGet]
        public JsonResult GetListCustomerIndividual()
        {
            try
            {
                var result = mC.ListCustomerIndividual();
                return Json(new { statut = "OK", message = "List of customers Individual", data = JsonConvert.SerializeObject(result) }, JsonRequestBehavior.AllowGet);
            }
            
            catch (Exception ex)
            {
                return Json(new { statut = "KO", message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [SwaggerOperation("GetList")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [System.Web.Mvc.HttpGet]
        public JsonResult GetList()
        {
            try
            {
                var result = mC.ListCustomer();
                return Json(new { statut = "OK", message = "List of all customers", data = JsonConvert.SerializeObject(result) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { statut = "KO", message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [System.Web.Mvc.HttpGet]
        public JsonResult GetListSearch(string name)
        {
            try
            {
                // name = "ohn do";
                var result = mC.ListCustomerSearch(name);
                return Json(new { statut = "OK", message = "", data = JsonConvert.SerializeObject(result) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { statut = "KO", message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [System.Web.Mvc.HttpPost]
        public JsonResult PostUpdate(Customer customer)
        {
            try
            {
                MCData lc = new MCData();
                lc.DataItem_Customer = new List<Customer> { customer };
                switch (convertion.Verification(lc))
                {
                    case 111:
                        var result = mC.UpdateCustomer(customer);
                        return Json(new { statut = "OK", message = "Update with succes!" }, JsonRequestBehavior.AllowGet);
                    case 531:
                        return Json(new { statut = "KO", message = "Customer must be at least 18 years old or verify date format", code = 531 }, JsonRequestBehavior.AllowGet);
                    case 532:
                        return Json(new { statut = "KO", message = "Values must be greater than 0 or verify a decimal place", code = 532 }, JsonRequestBehavior.AllowGet);
                    default:
                        return Json(new { statut = "KO", message = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { statut = "KO", message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        } 
        [System.Web.Mvc.HttpGet]
        public JsonResult GetDownloadCSV(string file)
        {
            try
            {
                //var file = Path.Combine(Server.MapPath("~"), "App_Data");
                var csv = mC.ListCustomer();
                convertion.Csv(file, csv);
                return Json(new { statut = "OK", message = "The download file is succes!" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { statut = "KO", message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
