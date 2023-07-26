using Aspose.Pdf;
using Aspose.Pdf.Cloud.Sdk.Api;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using PdfTest.Models;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml.Linq;

namespace PdfTest.Controllers
{
    public class CloudController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public CloudController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("cloud/parse")]
        public IActionResult Parse([FromBody]Upload upload)
        {
            try
            {

                if (upload == null) return Error();
                if (upload.Ticket == null) return Error();
                var dataUrls = upload.Ticket.Split(",");
                var fileBase = dataUrls[dataUrls.Length - 1];
                var mi = new MemoryStream(Convert.FromBase64String(fileBase));
                var fileKey = Guid.NewGuid().ToString();
                PdfApi api = new PdfApi("b7cb90d868a9105cf41f1c11efbcc797", "ce94c202-bf2d-4c2a-b21f-569f99d8c9c6");
                
                var convert = api.PutPdfInRequestToHtml(Path.Combine("outputs", $"{fileKey}.zip"),file:mi);

                var download = api.DownloadFile(Path.Combine("outputs", $"{fileKey}.zip"));

                var zipArchive = new ZipArchive(download);

                zipArchive.ExtractToDirectory($"tempZips\\{fileKey}");

                var result = new Ticket()
                {
                    Document = System.IO.File.ReadAllText($"tempZips\\{fileKey}\\{fileKey}.html")
                };

                return Json(result);
            }
            catch (Exception ex)
            {
                return Error();

            }
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

}