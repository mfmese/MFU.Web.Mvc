using Application.Data;
using Framework.Messaging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Business.Handlers
{
    public class GetExcelFileHistoryListRequest : BaseRequest<ResponseOf<List<Models.ExcelFileHistory>>>
    {
        public int ExcelFileId { get; set; }
    }

    class GetExcelFileHistoryListHandler : BaseHandler<GetExcelFileHistoryListRequest, ResponseOf<List<Models.ExcelFileHistory>>>
    {
        MFUContext dbContext;

        public GetExcelFileHistoryListHandler(MFUContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected override Task Handle(CancellationToken cancellationToken)
        {
            var excelFileHistory = dbContext.ExcelFileHistory.Where(x => x.ExcelFileId == Request.ExcelFileId).OrderByDescending(x => x.StateDate).ToList();

            Response.Data = Mapper.Map<List<Models.ExcelFileHistory>>(excelFileHistory);

            Response.AddMessage("Your operation completed Successfully.", "", MessageTypes.Success, isEndUserMessage: true);

            return Response.SetSuccess().AsTask();
        }

    }

}
