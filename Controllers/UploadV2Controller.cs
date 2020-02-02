using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UploadFileSampler.Models;

namespace UploadFileSampler.Controllers
{
    public class UploadV2Controller : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<HomeController> _logger;
        public UploadV2Controller(ILogger<HomeController> logger, IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
            _logger = logger;
        }



        public IActionResult Index(string Name)
        {
            string UploadFolder = $"{_hostingEnvironment.WebRootPath}\\Uploads";
            string ImageName = $"{UploadFolder}\\{Name}";
            if (Request.Method == "POST")
            {
                foreach (var upload in Request.Form.Files)
                {
                    System.IO.Directory.CreateDirectory(UploadFolder);
                    using (var fileStream = new FileStream($"{UploadFolder}\\{Name}", FileMode.Create))
                    {
                        upload.CopyTo(fileStream);
                    }
                }
                return Json(new
                {
                    redirectTo = Url.Action("ThankYou", "UploadV2",new { Name = Name }),
                });
            }
            IEnumerable<InfoWithURL> files;
            files = Directory.GetFiles(UploadFolder).Select(x => new InfoWithURL() { FileInfo = new FileInfo(x), URL = $"http://{Request.Host}\\uploads\\{new FileInfo(x).Name}" });
            return View(files);
        }

        public IActionResult ThankYou(string Name)
        {
            string UploadFolder = $"{_hostingEnvironment.WebRootPath}\\Uploads";
            IEnumerable<InfoWithURL> files;
            files = Directory.GetFiles(UploadFolder).Select(x => new InfoWithURL() { FileInfo = new FileInfo(x), URL = $"http://{Request.Host}\\uploads\\{new FileInfo(x).Name}" });
            return View(files);
        }

    }
}