using Framework.MakerChecker;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Data.Entities
{
    internal partial class ExcelFile: MakerCheckerEntity
    {
        public int ExcelFileId { get; set; }
        public string FileName { get; set; }
        public int? FileStateId { get; set; }

        public string FileStateIdName { get {

                if (FileStateId == 0)
                    return "UnProcessed";
                else
                    return "Processed";
            } }
        public byte[] Body { get; set; }

        [ForeignKey("FileId")]
        public ICollection<MfuswiftInfo> MfuswiftInfoList { get; set; }
    }
}
