using Framework.MakerChecker;
using System;
using System.Collections.Generic;

namespace Application.Business.Models
{
    public partial class ExcelFile: MakerCheckerListItem
    {
        public string RefId { get; set; }
        public int ExcelFileId { get; set; }
        public string FileName { get; set; }
        public int? FileStateId { get; set; }
        public string FileStateIdName { get; set; }
        public byte[] Body { get; set; }
        public List<MfuswiftInfo> MfuswiftInfoList { get; set; }
    }
}
