using Framework.MakerChecker;

namespace Application.Data.Entities
{
    internal partial class ExcelFileHistory: MakerCheckerEntity
    {
        public int ExcelFileHistoryId { get; set; }
        public int ExcelFileId { get; set; }
        public string FileName { get; set; }
        public int? FileState { get; set; }
        public byte[] Body { get; set; }
    }
}
