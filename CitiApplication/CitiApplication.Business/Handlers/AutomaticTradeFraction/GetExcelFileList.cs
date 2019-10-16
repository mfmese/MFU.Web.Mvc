using Application.Data;
using Framework.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Business.Handlers
{
    public class GetExcelFileListRequest : BaseRequest<ResponseOf<List<Models.ExcelFile>>>
    {
        public List<int> StateIds { get; set; }
        public DateTime StateDate { get; set; }
    }

    class GetExcelFileListHandler : BaseHandler<GetExcelFileListRequest, ResponseOf<List<Models.ExcelFile>>>
    {
        MFUContext dbContext;

        public GetExcelFileListHandler(MFUContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected override Task Handle(CancellationToken cancellationToken)
        {
            Response.Data = new List<Models.ExcelFile>();

            var excelFiles = dbContext.ExcelFile.WhereStateDateEquals(Request.StateDate).Where(o => o.StateId, Request.StateIds).ToList();

            Response.Data = Mapper.Map<List<Models.ExcelFile>>(excelFiles);

            int count = 1;
            foreach (var item in Response.Data)
            {
                item.RefId = count.ToString();
                count++;
            }

            Response.AddMessage("Your operation completed Successfully.", "", MessageTypes.Success, isEndUserMessage: true);

            return Response.SetSuccess().AsTask();
        }
    }

}
