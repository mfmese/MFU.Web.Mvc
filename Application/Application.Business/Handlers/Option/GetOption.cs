using Application.Business.Helpers;
using Application.Data;
using Framework.Messaging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Business.Handlers
{
    public class GetOptionRequest : BaseRequest<ResponseOf<List<Models.Option>>>
    {
    }

    class GetOptionHandler : BaseHandler<GetOptionRequest, ResponseOf<List<Models.Option>>>
    {
        MFUContext dbContext;
        MFUOperationsHelper mFUOperationsHelper;

        public GetOptionHandler(MFUContext dbContext, MFUOperationsHelper mFUOperationsHelper)
        {
            this.dbContext = dbContext;
            this.mFUOperationsHelper = mFUOperationsHelper;
        }

        protected override Task Handle(CancellationToken cancellationToken)
        {
            var optionList = dbContext.Option.ToList();

            Response.Data = Mapper.Map<List<Models.Option>>(optionList);

            Response.AddMessage("Your operation completed Successfully.", "", MessageTypes.Success, isEndUserMessage: true);

            return Response.SetSuccess().AsTask();
        }
    }
}
