using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using paytm;

namespace PaytmIntegration.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {


            Dictionary<String, String> paytmParams = new Dictionary<String, String>();
            String merchantMid = "rxazcv89315285244163";
            // Key in your staging and production MID available in your dashboard
            String merchantKey = "gKpu7IKaLSbkchFS";
            // Key in your staging and production merchant key available in your dashboard
            String orderId = "order1";
            String channelId = "WEB";
            String custId = "cust123";
            String mobileNo = "9712995395";
            String email = "abh@narola.email";
            String txnAmount = "100.12";
            String website = "WEBSTAGING";
            // This is the staging value. Production value is available in your dashboard
            String industryTypeId = "Retail";
            // This is the staging value. Production value is available in your dashboard
            String callbackUrl = "http://localhost:52528/Home/Index";
            paytmParams.Add("MID", merchantMid);
            paytmParams.Add("CHANNEL_ID", channelId);
            paytmParams.Add("WEBSITE", website);
            paytmParams.Add("CALLBACK_URL", callbackUrl);
            paytmParams.Add("CUST_ID", custId);
            paytmParams.Add("MOBILE_NO", mobileNo);
            paytmParams.Add("EMAIL", email);
            paytmParams.Add("ORDER_ID", orderId);
            paytmParams.Add("INDUSTRY_TYPE_ID", industryTypeId);
            paytmParams.Add("TXN_AMOUNT", txnAmount);
            // for staging 
            string transactionURL = "https://securegw-stage.paytm.in/theia/processTransaction";
            // for production 
            // string transactionURL = "https://securegw.paytm.in/theia/processTransaction"; 
            try
            {
                string paytmChecksum = paytm.CheckSum.generateCheckSum(merchantKey, paytmParams);
                string outputHTML = "<html>";
                outputHTML += "<head>";
                outputHTML += "<title>Merchant Checkout Page</title>";
                outputHTML += "</head>";
                outputHTML += "<body>";
                outputHTML += "<center><h1>Please do not refresh this page...</h1></center>";
                outputHTML += "<form method='post' action='" + transactionURL + "'  name='f1'>";
                foreach (string key in paytmParams.Keys)
                {
                    outputHTML += "<input type='hidden' name='" + key + "' value='" + paytmParams[key] + "'>";
                }
                outputHTML += "<input type='hidden' name='CHECKSUMHASH' value='" + paytmChecksum + "'>";
                outputHTML += "<script type='text/javascript'>";
                outputHTML += "document.f1.submit();";
                outputHTML += "</script>";
                outputHTML += "</form>";
                outputHTML += "</body>";
                outputHTML += "</html>";
                Response.Write(outputHTML);
            }
            catch (Exception ex)
            {
                Response.Write("Exception message: " + ex.Message.ToString());
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection fc)
        {
            String merchantKey = "gKpu7IKaLSbkchFS";
            Dictionary<String, String> paytmParams = new Dictionary<String, String>();
            string paytmChecksum = "";
            foreach (string key in Request.Form.Keys)
            {
                paytmParams.Add(key.Trim(), Request.Form[key].Trim());
            }
            if (paytmParams.ContainsKey("CHECKSUMHASH"))
            {
                paytmChecksum = paytmParams["CHECKSUMHASH"];
                paytmParams.Remove("CHECKSUMHASH");
            }
            bool isValidChecksum = CheckSum.verifyCheckSum(merchantKey, paytmParams, paytmChecksum);
            if (isValidChecksum)
            {
                Response.Write("Checksum Matched");
            }
            else
            {
                Response.Write("Checksum MisMatch");
            }
            return RedirectToAction("About");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            String transactionURL = "https://securegw-stage.paytm.in/merchant-status/getTxnStatus";
            String merchantKey = "gKpu7IKaLSbkchFS";
            String merchantMid = "rxazcv89315285244163";
            String orderId = "order1";
            Dictionary<String, String> paytmParams = new Dictionary<String, String>();
            paytmParams.Add("MID", merchantMid);
            paytmParams.Add("ORDERID", orderId);
            try
            {
                string paytmChecksum = paytm.CheckSum.generateCheckSum(merchantKey, paytmParams);
                paytmParams.Add("CHECKSUMHASH", paytmChecksum);
                String postData = "JsonData=" + new JavaScriptSerializer().Serialize(paytmParams);
                HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(transactionURL);
                connection.Headers.Add("ContentType", "application/json");
                connection.Method = "POST";
                using (StreamWriter requestWriter = new StreamWriter(connection.GetRequestStream()))
                {
                    requestWriter.Write(postData);
                }
                string responseData = string.Empty;
                using (StreamReader responseReader = new StreamReader(connection.GetResponse().GetResponseStream()))
                {
                    responseData = responseReader.ReadToEnd();
                    Response.Write(responseData);
                    Response.Write("Requested Json= " + postData);
                }
            }
            catch (Exception ex)
            {
                Response.Write("Exception message: " + ex.Message.ToString());
            }
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}