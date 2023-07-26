using Aspose.Pdf;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using PdfTest.Models;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace PdfTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("home/parse")]
        public IActionResult Parse([FromBody]Upload upload)
        {
            if (upload == null) return Error();
            if(upload.Ticket == null) return Error();
            var dataUrls = upload.Ticket.Split(",");
            var fileBase = dataUrls[dataUrls.Length - 1];
            var mi = new MemoryStream(Convert.FromBase64String(fileBase));
            // load PDF with an instance of Document                        
            var document = new Document(mi);

            document.Save("output.html", Aspose.Pdf.SaveFormat.Html);
            //// save document in HTML format
            //var mo = new MemoryStream();
            //document.Save(mo, Aspose.Pdf.SaveFormat.Html);
            //var content = Encoding.UTF8.GetString(mo.ToArray());
            var result = new Ticket() {
                Document = System.IO.File.ReadAllText("output.html")
            };
            return Json(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
    public class Upload { 
        public string Ticket { get; set; }
    }
    public class Ticket
    {
        public string TravelName { get; set; }
        public List<SectionTicket> Sections { get; set; }
        public string Document { get; set; }

    }
    public class SectionTicket {

        public string Plan { get; set; }
        public string Time { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}