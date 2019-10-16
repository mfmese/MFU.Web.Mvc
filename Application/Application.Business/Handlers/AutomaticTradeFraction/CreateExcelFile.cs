using Application.Business.Helpers;
using Application.Data;
using Application.Data.Entities;
using Citibank.MFU.Web.Business;
using FluentValidation;
using Framework.MakerChecker;
using Framework.Messaging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Business.Handlers
{

    public class CreateExcelFileRequest : Framework.Messaging.BaseRequest<VoidResponse>
    {
        public string FileName { get; set; }
        public byte[] Body { get; set; }     
    }

    internal class CreateExcelFileRequestValidator : AbstractValidator<CreateExcelFileRequest>
    {
        public CreateExcelFileRequestValidator()
        {
            RuleFor(o => o.FileName).NotNull().WithMessage("File Name is not valid");
        }
    }

    class CreateExcelFileHandler : MakerCheckerHandler<CreateExcelFileRequest, VoidResponse, Data.Entities.ExcelFile>
    {
        MFUContext dbContext;
        MFUOperationsHelper mFUOperationsHelper;

        public CreateExcelFileHandler(MFUContext dbContext, MFUOperationsHelper mFUOperationsHelper)
        {
            this.dbContext = dbContext;
            this.mFUOperationsHelper = mFUOperationsHelper;
        }

        protected override OperationType OperationType => OperationType.INSERT;

        protected override Data.Entities.ExcelFile GetEntity() => new ExcelFile();

        protected override Task Handle(CancellationToken cancellationToken)
        {
            bool isExists = dbContext.ExcelFile.Where(o => o.FileName == Request.FileName).Any();

            if (isExists)
            {
                return Response.SetFailed($"This excel file \"{Request.FileName}\" is already exists", "EXIST", true).AsTask();
            }
            else
            {
                base.Mapper.Map(Request, Entity);
                Entity.Body = Request.Body;
                Entity.FileStateId = 0;

                var excelFileId = InsertExcelFile(Entity);

                var stream = new MemoryStream(Request.Body);
                IList<MfuswiftInfo> mfuswiftInfoList = mFUOperationsHelper.GetMfuswiftInfoList(FileHelper.ExcelExportDataTable(stream));
                mFUOperationsHelper.InsertMfuswiftInfoList(mfuswiftInfoList, excelFileId, dbContext);

                Response.AddMessage("New File Created Successfully.", "", MessageTypes.Success, isEndUserMessage : true);

                return Response.SetSuccess().AsTask();
            }
        }

        private int InsertExcelFile(ExcelFile excelFile)
        {
            var result = dbContext.Add(excelFile);
            dbContext.SaveChanges();
            mFUOperationsHelper.InsertExcelFileHistory(result.Entity, dbContext);
            return result.Entity.ExcelFileId;
        }

    }
   
}
