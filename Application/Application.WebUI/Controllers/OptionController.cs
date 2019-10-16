using Application.Business.Handlers;
using Application.WebUI.Controllers;
using Application.WebUI.Models;
using Framework.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Citibank.MFU.Web.Controllers
{
    public class OptionController : BaseController
    {
        public IActionResult Index()
        {
            var viewModel = new OptionViewModel();

            viewModel.GridModel = new GridCrudModel
            {
                ControllerName = "Option",
                Key = "OptionId",
                LoadAction = nameof(Load),
                UpdateAction = nameof(Update),
                HistoryAction = nameof(History)
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Load(OptionViewModel viewModel)
        {
            var response = await Mediator.Send(new GetOptionRequest());

            if (response.IsFailed())
                return Json(response);

            return Json(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateOptionRequest request)
        {
            var response = await Mediator.Send(request);

            return Json(response);
        }

        public async Task<IActionResult> History(int optionId)
        {
            var response = await Mediator.Send(new GetOptionHistoriesRequest { OptionId = optionId });

            if (response.IsFailed())
                return Json(response);

            return Json(response.Data);
        }
    }
}