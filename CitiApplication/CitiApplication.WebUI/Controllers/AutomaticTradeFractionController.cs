using Application.Business.Handlers;
using Application.WebUI.Controllers;
using Application.WebUI.Models;
using DevExtreme.AspNet.Mvc;
using Framework.MakerChecker;
using Framework.WebUI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace Citibank.MFU.Web.Controllers
{
	public class AutomaticTradeFractionController : BaseController
    {

        public IActionResult Index()
        {
            var viewModel = new AutomaticTradeFractionViewModel();

            viewModel.GridModel = new GridCrudModel
            {
                ControllerName = "AutomaticTradeFraction",
                Key = "ExcelFileId",
                LoadAction = nameof(Load),
                InsertAction = nameof(Create),
                UpdateAction = nameof(CreateOrUpdate),
                DeleteAction = nameof(Delete),
                VerifyAction = nameof(Verify),
                RejectAction = nameof(Reject),
                DisplayAction = nameof(Load),
                HistoryAction = nameof(History),
                PrintAction = nameof(History),
                PopupTitle = "Automatic Trade Fraction"
            };

            viewModel.StateMaker = RequestContext.ContextUser.UserID;

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Load(AutomaticTradeFractionViewModel viewModel)
        {
            var stateIdList = States.GetStateIdList(viewModel.StateFilter);

            var response = await Mediator.Send(new GetExcelFileListRequest { StateIds = stateIdList, StateDate = viewModel.StateDate });

            if (response.IsFailed())
                return Json(response);

            return Json(response.Data);
        }

        [HttpGet]
        public async Task<IActionResult> LoadDetail(int excelFileId, DataSourceLoadOptions loadOption)
        {
            var response = await Mediator.Send(new GetMfuSwiftInfoListRequest { ExcelFileId = excelFileId });

            if (response.IsFailed())
                return BadRequest(response.GetMessagesAsString());

            return Json(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Create(GridJsonModel<CreateExcelFileRequest> gridJsonModel)
        {
            //CreateOrUpdate is running, this is just for enable create button on the page
            // This is manipulated in JS with function named function fileonValueChanged(e)
            return Json(null);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdate(int excelFileId, IFormFile file) 
        {
            byte[] body; 
            using (MemoryStream ms = new MemoryStream())
            {
                file.CopyTo(ms);
                body =  ms.ToArray();
            }

            //Create
            if(excelFileId == 0)
            {
                var request = new CreateExcelFileRequest();
                request.Body = body;
                request.FileName = file.FileName;

                var response = await Mediator.Send(request);
                return Json(response);
            }
            //Update
            else
            {
                var request = new UpdateExcelFileRequest();
                request.Body = body;
                request.FileName = file.FileName;
                request.ExcelFileId = excelFileId;

                var response = await Mediator.Send(request);
                return Json(response);
            }
        }

        public async Task<IActionResult> Delete(int key)
        {
            var response = await Mediator.Send(new DeleteExcelFileRequest() { ExcelFileId = key });

            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> Verify(VerifyExcelFileRequest request)
        {
            var response = await Mediator.Send(request);

            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> Reject(RejectExcelFileRequest request)
        {
            var response = await Mediator.Send(request);

            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> History(int excelFileId)
        {
            var response = await Mediator.Send(new GetExcelFileHistoryListRequest { ExcelFileId = excelFileId });

            if (response.IsFailed())
                return Json(response);

            return Json(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Export(int excelFileId)
        {
            var response = await Mediator.Send(new ExportExcelFileRequest { ExcelFileId = excelFileId });

            if (response.IsFailed())
            {
                return Json(response);
            }

            TempData["ExportedFile"] = response.Data;
            TempData["ExportedFileName"] = "SWIFT MESSAGES.zip";
            TempData["ExportedFileType"] = "application/zip";

            return Json(response);
        }

        [HttpGet]
        public IActionResult GetExportedFile()
        {
            byte[] bytes = (byte[])TempData["ExportedFile"];
            string fileName = (string)TempData["ExportedFileName"];
            string fileType = (string)TempData["ExportedFileType"];

            return File(bytes, fileType, fileName);         
        }

        public override IActionResult Error()
        {
            return base.Error();
        }

    }
}