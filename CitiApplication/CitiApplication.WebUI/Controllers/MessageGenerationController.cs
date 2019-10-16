using Application.Business.Handlers;
using Application.Business.Models;
using Application.WebUI.Controllers;
using Application.WebUI.Models;
using Framework.WebUI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Citibank.MFU.Web.Controllers
{
    public class MessageGenerationController : BaseController
    {
        public IActionResult Index()
        {
            var viewModel = new MessageGenerationViewModel();

            viewModel.GridModel = new GridCrudModel
            {
                ControllerName = "MessageGeneration",
                Key = "FileId",
                LoadAction = nameof(Load),
                //ImportAction = nameof(Import)
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Load(MessageGenerationViewModel request)
        {
            if (request.BeginDate == DateTime.MinValue)
            {
                request = new MessageGenerationViewModel
                {
                    BeginDate = DateTime.Now,
                    EndDate = DateTime.Now
                };
            }

            var response = await Mediator.Send(new GetMfuFilesRequest { BeginDate = request.BeginDate, EndDate = request.EndDate });

            if (response.IsFailed())
                return BadRequest(response.GetMessagesAsString());

            return Json(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> ImportMfuFile(IFormFileCollection files)
        {
            var request = new ImportMfuFileRequest();

            if (files.Count > 0)
            {
                byte[] body;
                using (MemoryStream ms = new MemoryStream())
                {
                    files[0].CopyTo(ms);
                    body = ms.ToArray();
                }

                request.ImportedFile = new FileItem { File = body, FileName = files[0].FileName };
            }

            var response = await Mediator.Send(request);

            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> ExportMfuFile(int fileId)
        {
            var response = await Mediator.Send(new ExportMfuFileRequest { FileId = fileId });

            return Json(response);
        }
    }
}