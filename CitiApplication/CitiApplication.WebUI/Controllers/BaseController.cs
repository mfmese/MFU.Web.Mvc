using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Framework.Logging;
using Framework.Messaging;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Application.WebUI.Controllers
{
    public class BaseController : Framework.WebUI.Controllers.BaseController
    {

        private IMapper _mapper;
        protected IMapper mapper => _mapper ?? (_mapper = HttpContext.RequestServices.GetService<IMapper>());

        public BaseController() { }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //Context.ContextUser = new ContextUser
            //{
            //    UserID = "be94878", 
            //    Username = "Mehmet Fethi"
            //};
            base.OnActionExecuting(context);
        }

        public override IActionResult Error()
		{
			return base.Error();
		}
	}
}