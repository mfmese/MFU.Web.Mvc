using Framework.MakerChecker;

namespace Application.Business.Models
{
    public partial class ExcelFileHistory: MakerCheckerListItem
    {
        public int ExcelFileHistoryId { get; set; }
        public int ExcelFileId { get; set; }
        public string FileName { get; set; }
        public int? FileState { get; set; }
        public byte[] Body { get; set; }
    }
}
