using Application.Business.Helpers;
using Application.Data;
using FluentValidation;
using Framework.MakerChecker;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Business.Handlers
{

    public class DeleteExcelFileRequest : Framework.Messaging.BaseRequest<VoidResponse>
    {
        public int ExcelFileId { get; set; }
    }

    internal class DeleteExcelFileRequestValidator : AbstractValidator<DeleteExcelFileRequest>
    {
        public DeleteExcelFileRequestValidator()
        {
            //RuleFor(o => o.ExcelFileId).NotNull().WithMessage("Excel File Id  not valid");
        }
    }

    class DeleteExcelFileHandler : MakerCheckerHandler<DeleteExcelFileRequest, VoidResponse, Data.Entities.ExcelFile> 
    {
        MFUContext dbContext;
        MFUOperationsHelper mFUOperationsHelper;

        public DeleteExcelFileHandler(MFUContext dbContext, MFUOperationsHelper mFUOperationsHelper)
        {
            this.dbContext = dbContext;
            this.mFUOperationsHelper = mFUOperationsHelper;
        }

        protected override OperationType OperationType => OperationType.DELETE;

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

                return Response.SetSuccessWithEndUserMessage("File Deleted Successfully.","").AsTask();
            }
        }
    }
}
