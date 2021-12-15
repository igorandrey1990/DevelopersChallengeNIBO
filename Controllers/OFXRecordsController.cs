using DevelopersChallengeNIBO.Interfaces.Services;
using DevelopersChallengeNIBO.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using Vereyon.Web;

namespace DevelopersChallengeNIBO.Controllers
{
    public class OFXRecordsController : Controller
    {
        private readonly IOFXRecordsService _OFXRecordsService;
        private readonly IFlashMessage _FlashMessage;

        public OFXRecordsController(IOFXRecordsService OFXRecordsService, IFlashMessage FlashMessage)
        {
            _OFXRecordsService = OFXRecordsService;
            _FlashMessage = FlashMessage;
        }

        public IActionResult Index()
        {
            // Gets all records from DB
            List<OFXRecord> records = _OFXRecordsService.Get();
            return View(records);
        }

        public IActionResult DeleteAll()
        {
            try
            {
                _OFXRecordsService.DeleteAll();
            }
            catch (Exception ex)
            {
                return Content("There was an error: " + ex);
            }

            return RedirectToAction("Index");
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
                if (file == null)
                {
                    _FlashMessage.Warning("Please Select a File!");
                    return RedirectToAction("UploadFile");
                }

                string path = Path.Combine(Directory.GetCurrentDirectory(), "OFX Files", file.FileName);

                if (System.IO.File.Exists(path))
                {
                    _FlashMessage.Warning("File already exists!");
                    return RedirectToAction("UploadFile");
                }
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    file.CopyToAsync(fileStream);
                }

                _OFXRecordsService.ImportOFX(path);
            }
            catch (Exception ex)
            {
                return Content("There was an error: " + ex);
            }

            return RedirectToAction("Index");
        }
    }
}
