using Application.Business.Helpers;
using Application.Data;
using Framework.MakerChecker;
using Framework.Messaging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Business.Handlers
{
    public class RejectExcelFileRequest : Framework.Messaging.BaseRequest<VoidResponse>
    {
        public int ExcelFileId { get; set; }
    }

    class RejectExcelFileHandler : MakerCheckerHandler<RejectExcelFileRequest, VoidResponse, Data.Entities.ExcelFile> 
    {
        MFUContext dbContext;
        MFUOperationsHelper mFUOperationsHelper;
        public RejectExcelFileHandler(MFUContext dbContext, MFUOperationsHelper mFUOperationsHelper)
        {
            this.dbContext = dbContext;
            this.mFUOperationsHelper = mFUOperationsHelper;
        }

        protected override OperationType OperationType => OperationType.REJECT;

        protected override Data.Entities.ExcelFile GetEntity()
        {
            return dbContext.ExcelFile.FirstOrDefault(x => x.ExcelFileId == Request.ExcelFileId);
        }

        protected override Task Handle(CancellationToken cancellationToken)
        {
            if (Entity == null)
            {
                return Response.SetFailed("Record can not be found.", "NotFound", true).AsTask();
            }
            else
            {
                dbContext.Update(Entity);

                dbContext.SaveChanges();

                mFUOperationsHelper.InsertExcelFileHistory(Entity, dbContext);

                return Response.SetSuccessWithEndUserMessage("Rejected Successfully.", "").AsTask();
            }
        }

    }
    

}
