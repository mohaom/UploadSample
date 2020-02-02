using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UploadFileSampler.Models;

namespace UploadFileSampler.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
            _logger = logger;
        }

        public IActionResult Index()
        {
            string UploadFolder = $"{_hostingEnvironment.WebRootPath}\\Uploads";
            if (Request.Method == "POST")
                foreach (var upload in Request.Form.Files)
                {
                    System.IO.Directory.CreateDirectory(UploadFolder);
                    using (var fileStream = new FileStream($"{UploadFolder}\\{upload.FileName}", FileMode.Create))
                    {
                        upload.CopyTo(fileStream);
                    }

                }
            IEnumerable<InfoWithURL> files;
            files = Directory.GetFiles(UploadFolder).Select(x => new InfoWithURL() { FileInfo = new FileInfo(x), URL = $"http://{Request.Host}\\uploads\\{new FileInfo(x).Name}" });
            return View(files);
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
        public async Task<IActionResult> Delete(string id)
        {
            string UploadFolder = $"{_hostingEnvironment.WebRootPath}\\Uploads";
            System.IO.File.Delete($"{UploadFolder}\\{id}");
            IEnumerable<InfoWithURL> files;
            files = Directory.GetFiles(UploadFolder).Select(x => new InfoWithURL() { FileInfo = new FileInfo(x), URL = $"http://{Request.Host}\\uploads\\{new FileInfo(x).Name}" });
            return View("Index",files);
        }
    }
}
