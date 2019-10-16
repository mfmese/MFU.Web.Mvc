using Application.Data;
using Framework.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Business.Handlers
{
    public class GetMfuFilesRequest : BaseRequest<ResponseOf<List<Models.Mfufile>>>
    {
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    class GetMfuFilesHandler : BaseHandler<GetMfuFilesRequest, ResponseOf<List<Models.Mfufile>>>
    {
        MFUContext dbContext;

        public GetMfuFilesHandler(MFUContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected override Task Handle(CancellationToken cancellationToken)
        {
            var mfuFileList = dbContext.Mfufile.Where(x => x.EnterDate.Date >= Request.BeginDate.Date && x.EnterDate.Date <= Request.EndDate.Date).ToList();

            Response.Data = Mapper.Map<List<Models.Mfufile>>(mfuFileList);

            return Response.SetSuccessWithEndUserMessage("Your operation completed Successfully.", "OK").AsTask();
        }
    }
}
