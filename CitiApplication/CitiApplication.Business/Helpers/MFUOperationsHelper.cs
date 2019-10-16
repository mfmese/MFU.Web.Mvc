using Application.Data.Entities;
using Citibank.MFU.Web.Business;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;

namespace Application.Business.Helpers
{
    internal class MFUOperationsHelper
    {
        public IList<MfuswiftInfo> GetMfuswiftInfoList(DataTable dataTable)
        {
            if (dataTable == null)
                return null;

            var mfuswiftInfoList = new List<MfuswiftInfo>();

            foreach (DataRow r in dataTable.Rows)
            {
                var mfuswiftInfo = new MfuswiftInfo
                {
                    Acc = r.ItemArray[0].ToString(),
                    TradeDate = NullHelper.NullCheckDate(r.ItemArray[1]),
                    SettDate = NullHelper.NullCheckDate(r.ItemArray[2]),
                    Isin = r.ItemArray[3].ToString(),
                    Diff = NullHelper.NullCheckDecimal(r.ItemArray[4]),
                    Amount = NullHelper.NullCheckDecimal(r.ItemArray[5]),
                    Price = NullHelper.NullCheckDecimal(r.ItemArray[6]),
                    Refe = r.ItemArray[7].ToString(),
                    Act = r.ItemArray[8].ToString(),
                    TradeType = r.ItemArray[9].ToString()
                };
                mfuswiftInfoList.Add(mfuswiftInfo);
            }
            return mfuswiftInfoList;
        }

        public void InsertExcelFileHistory(ExcelFile excelFile, DbContext DbContext)
        {
            var excelFileHistory = new ExcelFileHistory
            {
                Body = excelFile.Body,
                StateId = excelFile.StateId,
                ExcelFileId = excelFile.ExcelFileId,
                FileName = excelFile.FileName,
                FileState = excelFile.FileStateId,
                StateDate = excelFile.StateDate,
                StateMaker = excelFile.StateMaker
            };

            DbContext.Add(excelFileHistory);
            DbContext.SaveChanges();
        }

        public void InsertMfuswiftInfoList(IList<MfuswiftInfo> mfuswiftInfoList, int excelFileId, DbContext dbContext)
        {
            foreach (var mfuswiftInfo in mfuswiftInfoList)
            {
                mfuswiftInfo.FileId = excelFileId;
                dbContext.Add(mfuswiftInfo);
            }
            dbContext.SaveChanges();
        }

        public void UpdateExcelFile(ExcelFile excelFile, DbContext dbContext)
        {
            var result = dbContext.Update(excelFile);
            dbContext.SaveChanges();
            InsertExcelFileHistory(excelFile, dbContext);
        }
    }
}
