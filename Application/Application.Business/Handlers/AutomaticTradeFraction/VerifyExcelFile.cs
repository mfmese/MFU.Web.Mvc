using Application.Business.Helpers;
using Application.Data;
using FluentValidation;
using Framework.MakerChecker;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Business.Handlers
{

    public class VerifyExcelFileRequest : Framework.Messaging.BaseRequest<VoidResponse>
    {
        public int ExcelFileId { get; set; }
    }

    internal class VerifyExcelFileRequesttValidator : AbstractValidator<VerifyExcelFileRequest>
    {
        public VerifyExcelFileRequesttValidator()
        {

        }
    }

    class VerifyExcelFileHandler : MakerCheckerHandler<VerifyExcelFileRequest, VoidResponse, Data.Entities.ExcelFile>
    {
        MFUContext dbContext;
        MFUOperationsHelper mFUOperationsHelper;

        public VerifyExcelFileHandler(MFUContext dbContext, MFUOperationsHelper mFUOperationsHelper)
        {
            this.dbContext = dbContext;
            this.mFUOperationsHelper = mFUOperationsHelper;
        }

        protected override OperationType OperationType => OperationType.VERIFY;

        protected override Data.Entities.ExcelFile GetEntity()
        {
            return dbContext.ExcelFile.FirstOrDefault(o => o.ExcelFileId == Request.ExcelFileId);
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

                return Response.SetSuccessWithEndUserMessage("Verified Successfully.", "").AsTask();
            }
        }
    }


}
