using Application.Data;
using Framework.Messaging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Business.Handlers
{
    public class ExportMfuFileRequest : BaseRequest<ResponseOf<List<Models.Mfumodel>>>
    {
        public int FileId { get; set; }
    }

    class ExportMfuFileHandler : BaseHandler<ExportMfuFileRequest, ResponseOf<List<Models.Mfumodel>>>
    {
        MFUContext dbContext;

        public ExportMfuFileHandler(MFUContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected override Task Handle(CancellationToken cancellationToken)
        {
            Response.Data = GenerateFile(Request.FileId); //TODO:  template file not exist so could not implemented

            //if (Response.IsFailed())
            //    return Task.FromResult(Response);

            Response.AddMessage("Export Action Not Implemented.", "", MessageTypes.Error, isEndUserMessage: true);

            return Response.SetFailed().AsTask();
        }

        private List<Models.Mfumodel> GenerateFile(int fileId)
        {
            var response = new List<Models.Mfumodel>();

            var mfuModel = dbContext.Mfumodel.Where(x => x.FileId == fileId).ToList();

            //FileHelper.ZipFiles(fileResponse.FileNameBodyList);

            return response;
        }
    }
}
