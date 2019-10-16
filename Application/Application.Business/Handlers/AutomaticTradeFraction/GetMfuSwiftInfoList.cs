using Application.Data;
using Framework.Messaging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Business.Handlers
{
    public class GetMfuSwiftInfoListRequest : BaseRequest<ResponseOf<List<Models.MfuswiftInfo>>>
    {
        public int ExcelFileId { get; set; }
    }

    class GetMfuSwiftInfoListHandler : BaseHandler<GetMfuSwiftInfoListRequest, ResponseOf<List<Models.MfuswiftInfo>>>
    {
        MFUContext dbContext;

        public GetMfuSwiftInfoListHandler(MFUContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected override Task Handle(CancellationToken cancellationToken)
        {
            var mfuSwiftInfoList = dbContext.MfuswiftInfo.Where(x => x.FileId == Request.ExcelFileId).OrderByDescending(x => x.TradeDate).ToList();

            Response.Data = Mapper.Map<List<Models.MfuswiftInfo>>(mfuSwiftInfoList);

            Response.AddMessage("Your operation completed Successfully.", "", MessageTypes.Success, isEndUserMessage: true);

            return Response.SetSuccess().AsTask();
        }
    }
}
