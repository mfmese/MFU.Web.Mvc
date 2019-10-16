using Application.Data;
using Application.Data.Entities;
using Citibank.MFU.Web.Business;
using Framework.Messaging;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Business.Handlers
{
    public class ImportMfuFileRequest : BaseRequest<ResponseOf<List<Models.Mfumodel>>>
    {
        public Models.FileItem ImportedFile { get; set; }
    }

    class ImportMfuFileHandler : BaseHandler<ImportMfuFileRequest, ResponseOf<List<Models.Mfumodel>>>
    {
        MFUContext dbContext;
        IConfiguration configuration;
        public ImportMfuFileHandler(MFUContext dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
        }

        protected override Task Handle(CancellationToken cancellationToken)
        {
            var mfuFile = new Mfufile()
            {
                EnterDate = DateTime.Now,
                FileName = configuration.GetValue<string>("FileName:RP147")
            };

            var mfuFileResult = dbContext.Add(mfuFile);

            MemoryStream fileStream = new MemoryStream(Request.ImportedFile.File);

            var mfumodelList = GetMfuModelList(FileHelper.ExcelExportDataTable(fileStream));
            foreach (var mfumodel in mfumodelList)
            {
                mfumodel.FileId = mfuFileResult.Entity.FileId;
                dbContext.Mfumodel.Add(mfumodel);
            }

            dbContext.SaveChanges();

            Response.AddMessage("File Imported Successfully.", "", MessageTypes.Success, isEndUserMessage: true);

            return Response.SetSuccess().AsTask();
        }

        private List<Mfumodel> GetMfuModelList(DataTable dataTable)
        {
            if (dataTable == null)
                return null;

            var mfumodelList = new List<Mfumodel>();

            foreach (DataRow r in dataTable.Rows)
            {
                var mfumodel = new Mfumodel
                {
                    AccountNo = r.ItemArray[0].ToString(),
                    Member = r.ItemArray[1].ToString(),
                    Isin = r.ItemArray[2].ToString(),
                    DefineDetail = r.ItemArray[3].ToString(),
                    BeginOfDay = NullHelper.NullCheckDecimal(r.ItemArray[4]),
                    ExchangeDate = NullHelper.NullCheckDate(r.ItemArray[5]),
                    Balance = NullHelper.NullCheckDecimal(r.ItemArray[6]),
                    DebtOrMoneyOwedToOne = r.ItemArray[7].ToString(),
                    ClearingType = r.ItemArray[8].ToString(),
                };
                mfumodelList.Add(mfumodel);
            }
            return mfumodelList;
        }
    }
}
