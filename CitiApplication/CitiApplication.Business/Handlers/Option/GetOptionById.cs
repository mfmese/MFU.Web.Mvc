using Application.Business.Helpers;
using Application.Data;
using Framework.Messaging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Business.Handlers
{
    public class GetOptionByIdRequest : BaseRequest<ResponseOf<Models.Option>>
    {
        public int OptionId { get; set; }
    }

    class GetOptionByIdHandler : BaseHandler<GetOptionByIdRequest, ResponseOf<Models.Option>>
    {
        MFUContext dbContext;
        MFUOperationsHelper mFUOperationsHelper;

        public GetOptionByIdHandler(MFUContext dbContext, MFUOperationsHelper mFUOperationsHelper)
        {
            this.dbContext = dbContext;
            this.mFUOperationsHelper = mFUOperationsHelper;
        }

        protected override Task Handle(CancellationToken cancellationToken)
        {
            var option = dbContext.Option.FirstOrDefault(x => x.OptionId == Request.OptionId);

            Response.Data = Mapper.Map<Models.Option>(option);

            Response.AddMessage("Your operation completed Successfully.", "", MessageTypes.Success, isEndUserMessage: true);

            return Response.SetSuccess().AsTask();
        }
    }
}
