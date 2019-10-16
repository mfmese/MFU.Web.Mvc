using Application.Business.Helpers;
using Application.Data;
using Application.Data.Entities;
using Citibank.MFU.Web.Business;
using FluentValidation;
using Framework.MakerChecker;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Business.Handlers
{
    public class UpdateExcelFileRequest : Framework.Messaging.BaseRequest<VoidResponse>
    {
        public int ExcelFileId { get; set; }
        public string FileName { get; set; }
        public byte[] Body { get; set; }
    }

    internal class UpdateExcelFileRequestValidator : AbstractValidator<UpdateExcelFileRequest>
    {
        public UpdateExcelFileRequestValidator()
        {

        }
    }

    class UpdateExcelFileHandler : MakerCheckerHandler<UpdateExcelFileRequest, VoidResponse, Data.Entities.ExcelFile>
    {
        MFUContext dbContext;
        MFUOperationsHelper mFUOperationsHelper;

        public UpdateExcelFileHandler(MFUContext dbContext, MFUOperationsHelper mFUOperationsHelper)
        {
            this.dbContext = dbContext;
            this.mFUOperationsHelper = mFUOperationsHelper;
        }

        protected override OperationType OperationType => OperationType.UPDATE;

        protected override Data.Entities.ExcelFile GetEntity()
        {
            return dbContext.ExcelFile.FirstOrDefault(o => o.ExcelFileId == Request.ExcelFileId);
        }

        protected override Task Handle(CancellationToken cancellationToken)
        {
            var stream = new MemoryStream(Request.Body);

            IList<MfuswiftInfo> mfuswiftInfoList = mFUOperationsHelper.GetMfuswiftInfoList(FileHelper.ExcelExportDataTable(stream));

            var excelFile = Mapper.Map(Request, Entity);

            mFUOperationsHelper.UpdateExcelFile(excelFile, dbContext);

            mFUOperationsHelper.InsertMfuswiftInfoList(mfuswiftInfoList, Request.ExcelFileId, dbContext);

            return Response.SetSuccessWithEndUserMessage("File Updated Successfully.", "OK").AsTask();
        }
    }
}

