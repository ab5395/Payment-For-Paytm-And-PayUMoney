using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PayUMoneyIntegration.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //var client = new RestClient("https://test.payumoney.com/payment/payment/chkMerchantTxnStatus?merchantKey=dOhSSgLh&merchantTransactionIds=Txn8479");
            //var request = new RestRequest(Method.POST);
            //request.AddHeader("postman-token", "2b511f14-fd76-5e59-dcf8-77b3b370831a");
            //request.AddHeader("cache-control", "no-cache");
            //request.AddHeader("authorization", "XT61/vMWnsfv0uKcdUtBOPptpzCWWXOY8PKUPvoMgPM=");
            //IRestResponse response = client.Execute(request);
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}