using Application.Business.Helpers;
using Application.Data;
using Application.Data.Entities;
using Citibank.MFU.Web.Business;
using Framework.Messaging;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Business.Handlers
{
    public class ExportExcelFileRequest : BaseRequest<ResponseOf<byte[]>>
    {
        public int ExcelFileId { get; set; }
    }

    class ExportExcelFileHandler : BaseHandler<ExportExcelFileRequest, ResponseOf<byte[]>>
    {
        MFUContext dbContext;
        MFUOperationsHelper mFUOperationsHelper;
        IConfiguration configuration;
        public ExportExcelFileHandler(MFUContext dbContext, MFUOperationsHelper mFUOperationsHelper, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.mFUOperationsHelper = mFUOperationsHelper;
            this.configuration = configuration;            
        }

        protected override Task Handle(CancellationToken cancellationToken)
        {
            string SwiftTypeDF = configuration.GetValue<string>("SwiftType:SwiftTypeDF"); 
            string SwiftTypeDVP = configuration.GetValue<string>("SwiftType:SwiftTypeDVP"); 

            var response = GenerateFile(Request.ExcelFileId, Context.ContextUser.UserID, SwiftTypeDVP, SwiftTypeDF);

            return Response.SetSuccess().AsTask();
        }

        private ResponseOf<byte[]> GenerateFile(int excelFileId, string userName, string swiftTypeDVP, string swiftTypeDF)
        {
            var fileResponse = new Models.FileResponse();

            var excelFile = dbContext.ExcelFile.FirstOrDefault(x => x.ExcelFileId == excelFileId);
            excelFile.StateMaker = userName;

            fileResponse = CheckFileState(excelFile);

            if (fileResponse.ResponseMessage != null) //fail
            {
                Response.AddMessage(fileResponse.ResponseMessage, "", MessageTypes.Error, isEndUserMessage: true);
                Response.SetFailed();
            }
            else //success
            {
                excelFile.FileStateId = Models.FileState.Processed.Id;
                mFUOperationsHelper.UpdateExcelFile(excelFile, dbContext);

                string userFirstTwhoChar = userName.ToString().Substring(0, 2).ToUpper();
                string excelFileIdPadLeft = excelFileId.ToString().PadLeft(5, '0') + ".txt";
                string dvpFileName = userFirstTwhoChar + "543" + excelFileIdPadLeft;
                string dfFileName = userFirstTwhoChar + "542" + excelFileIdPadLeft;

                var mfUSwiftInfoList = dbContext.MfuswiftInfo.Where(x => x.FileId == excelFileId).ToList();

                fileResponse.ResponseMessage = "Swift messages have been generated SUCCESFULLY. Please check the directory SWIFT MESSAGES.";
                fileResponse.FileNameBodyList.Add(new Models.FileNameBody { Name = dvpFileName, Body = GetDVPFile(mfUSwiftInfoList, dvpFileName, swiftTypeDVP) });
                fileResponse.FileNameBodyList.Add(new Models.FileNameBody { Name = dfFileName, Body = GetDFFile(mfUSwiftInfoList, dfFileName, swiftTypeDF) });

                Response.Data = FileHelper.ZipFiles(fileResponse.FileNameBodyList);

                Response.AddMessage(fileResponse.ResponseMessage, "", MessageTypes.Success, isEndUserMessage: true);

                Response.SetSuccess();
            }

            return Response;
        }

        private Models.FileResponse CheckFileState(ExcelFile excelFile)
        {
            var fileResponse = new Models.FileResponse();
            if (excelFile.StateId != Framework.MakerChecker.States.InputVerified.Id && excelFile.StateId != Framework.MakerChecker.States.EditVerified.Id)
            {
                fileResponse.ResponseMessage = "File is not verified, you can not generate";
                return fileResponse;
            }

            if (excelFile.FileStateId == Models.FileState.Processed.Id)
            {
                fileResponse.ResponseMessage = "File had already been generated";
                return fileResponse;
            }
            return fileResponse;
        }

        private byte[] GetDVPFile(List<MfuswiftInfo> mfUSwiftInfoList, string fileName, string swiftType)
        {
            var stringBuilder = new StringBuilder();
            var fileSizeKey = "[000000]";

            mfUSwiftInfoList = mfUSwiftInfoList.Where(x => x.TradeType.Trim() == swiftType).ToList();
            int lastCount = mfUSwiftInfoList.Count();
            if (lastCount > 0)
                stringBuilder = StringHelper.GetStringBuilder(fileName, fileSizeKey, lastCount.ToString().PadLeft(4, '0'));

            int i = 0;
            foreach (var mfUSwiftInfo in mfUSwiftInfoList)
            {
                i++;
                stringBuilder.AppendLine(i.ToString().PadLeft(13, '0'));
                stringBuilder.AppendLine("0000 00CITITRIXXXXX00000");
                stringBuilder.AppendLine("1355 08GCNLONSEAXXX00000");
                stringBuilder.AppendLine("543 02");
                stringBuilder.AppendLine(":16R:GENL");
                stringBuilder.AppendLine(":20C::SEME//" + mfUSwiftInfo.Refe);
                stringBuilder.AppendLine(":23G:NEWM");
                stringBuilder.AppendLine(":16S:GENL");
                stringBuilder.AppendLine(":16R:TRADDET");
                stringBuilder.AppendLine(":98A::SETT//" + mfUSwiftInfo.SettDate.Value.ToString("yyyyMMdd"));
                stringBuilder.AppendLine(":98A::TRAD//" + mfUSwiftInfo.TradeDate.Value.ToString("yyyyMMdd"));
                stringBuilder.AppendLine(":90B::DEAL//ACTU/TRY" + mfUSwiftInfo.Price.ToString().Replace('.', ','));
                stringBuilder.AppendLine(":35B:ISIN " + mfUSwiftInfo.Isin.ToString());
                stringBuilder.AppendLine(":16S:TRADDET");
                stringBuilder.AppendLine(":16R:FIAC");
                stringBuilder.AppendLine(":36B::SETT//UNIT/" + mfUSwiftInfo.Diff.ToString().Replace('.', ','));
                stringBuilder.AppendLine(":97A::SAFE//" + mfUSwiftInfo.Acc.ToString().PadLeft(6, '0'));
                stringBuilder.AppendLine(":16S:FIAC");
                stringBuilder.AppendLine(":16R:SETDET");
                stringBuilder.AppendLine(":22F::SETR//TRAD");
                stringBuilder.AppendLine(":16R:SETPRTY");
                stringBuilder.AppendLine(":95P::PSET//TVSBTRIS");
                stringBuilder.AppendLine(":16S:SETPRTY");
                stringBuilder.AppendLine(":16R:SETPRTY");
                stringBuilder.AppendLine(":95P::BUYR//CITITRIX");
                stringBuilder.AppendLine(":97A::SAFE//999999");
                stringBuilder.AppendLine(":16S:SETPRTY");
                stringBuilder.AppendLine(":16R:SETPRTY");
                stringBuilder.AppendLine(":95P::REAG//CITITRIX");
                stringBuilder.AppendLine(":16S:SETPRTY");
                stringBuilder.AppendLine(":16R:AMT");
                stringBuilder.AppendLine(":19A::SETT//TRY" + mfUSwiftInfo.Amount.ToString().Replace('.', ','));
                stringBuilder.AppendLine(":16S:AMT");
                stringBuilder.AppendLine(":16S:SETDET");
                stringBuilder.AppendLine("-|");
            }
            var byteArray = StringHelper.ConvertToByteArray(stringBuilder).Length;
            string text = stringBuilder.ToString().Replace(fileSizeKey, byteArray.ToString().PadLeft(8, '0'));
            return StringHelper.ConvertToByteArray(text);
        }

        private byte[] GetDFFile(List<MfuswiftInfo> mfUSwiftInfoList, string fileName, string swiftType)
        {
            var stringBuilder = new StringBuilder();
            var fileSizeKey = "[000000]";

            mfUSwiftInfoList = mfUSwiftInfoList.Where(x => x.TradeType.Trim() == swiftType).ToList();
            int lastCount = mfUSwiftInfoList.Count();
            if (lastCount > 0)
                stringBuilder = StringHelper.GetStringBuilder(fileName, fileSizeKey, lastCount.ToString().PadLeft(4, '0'));

            int i = 0;
            foreach (var mfUSwiftInfo in mfUSwiftInfoList)
            {
                i++;
                stringBuilder.AppendLine(i.ToString().PadLeft(13, '0'));
                stringBuilder.AppendLine("0000 00CITITRIXXXXX00000");
                stringBuilder.AppendLine("1355 08GCNLONSEAXXX00000");
                stringBuilder.AppendLine("542 02");
                stringBuilder.AppendLine(":16R:GENL");
                stringBuilder.AppendLine(":20C::SEME//" + mfUSwiftInfo.Refe);
                stringBuilder.AppendLine(":23G:NEWM");
                stringBuilder.AppendLine(":16S:GENL");
                stringBuilder.AppendLine(":16R:TRADDET");
                stringBuilder.AppendLine(":98A::SETT//" + mfUSwiftInfo.SettDate.Value.ToString("yyyyMMdd"));
                stringBuilder.AppendLine(":98A::TRAD//" + mfUSwiftInfo.TradeDate.Value.ToString("yyyyMMdd"));
                stringBuilder.AppendLine(":35B:ISIN " + mfUSwiftInfo.Isin.ToString());
                stringBuilder.AppendLine(":16S:TRADDET");
                stringBuilder.AppendLine(":16R:FIAC");
                stringBuilder.AppendLine(":36B::SETT//UNIT/" + mfUSwiftInfo.Diff.ToString().Replace('.', ','));
                stringBuilder.AppendLine(":97A::SAFE//" + mfUSwiftInfo.Acc.ToString().PadLeft(6, '0'));
                stringBuilder.AppendLine(":16S:FIAC");
                stringBuilder.AppendLine(":16R:SETDET");
                stringBuilder.AppendLine(":22F::SETR//TRAD");
                stringBuilder.AppendLine(":16R:SETPRTY");
                stringBuilder.AppendLine(":95P::PSET//TVSBTRIS");
                stringBuilder.AppendLine(":16S:SETPRTY");
                stringBuilder.AppendLine(":16R:SETPRTY");
                stringBuilder.AppendLine(":95P::BUYR//CITITRIX");
                stringBuilder.AppendLine(":97A::SAFE//999999");
                stringBuilder.AppendLine(":16S:SETPRTY");
                stringBuilder.AppendLine(":16R:SETPRTY");
                stringBuilder.AppendLine(":95P::REAG//CITITRIX");
                stringBuilder.AppendLine(":16S:SETPRTY");
                stringBuilder.AppendLine(":16S:SETDET");
                stringBuilder.AppendLine("-|");
            }
            var byteArray = StringHelper.ConvertToByteArray(stringBuilder).Length;
            string text = stringBuilder.ToString().Replace(fileSizeKey, byteArray.ToString().PadLeft(8, '0'));
            return StringHelper.ConvertToByteArray(text);
        }

    }





}
