using DevelopersChallengeNIBO.Interfaces.Services;
using DevelopersChallengeNIBO.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;

namespace DevelopersChallengeNIBO.Controllers
{
    public class OFXRecordsController : Controller
    {
        private readonly IOFXRecordsService _OFXRecordsService;
        private readonly IWebHostEnvironment _Envirnoment;

        public OFXRecordsController(IOFXRecordsService OFXRecordsService, IWebHostEnvironment Enviroment)
        {
            _OFXRecordsService = OFXRecordsService;
            _Envirnoment = Enviroment;
        }

        public IActionResult Index()
        {
            // Gets all records from DB
            List<OFXRecord> records = _OFXRecordsService.Get();
            return View(records);
        }

        public IActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Import(IFormFile file)
        {
            // Saves file path and calls import method from service then returns to Index view
            try
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", file.FileName);
                _OFXRecordsService.ImportOFX(path);
            } catch (Exception ex)
            {
                return Content("There was an error: " + ex);
            }

            return RedirectToAction("Index");
        }
    }
}
