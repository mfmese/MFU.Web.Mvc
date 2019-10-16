using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Citibank.MFU.Web.Business
{
    internal class FileHelper
    {
        public static DataTable ExcelExportDataTable(Stream fileStream)
        {
            Workbook workbook = new Workbook(fileStream);
            Worksheet worksheet = workbook.Worksheets[0];
            return worksheet.Cells.ExportDataTable(0, 0, worksheet.Cells.MaxRow + 1, worksheet.Cells.MaxColumn + 1, true);
        }
        public static byte[] ConvertFileToByteArray(string filePath)
        {
            MemoryStream memoryStream = new MemoryStream();
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                file.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
        public static byte[] ZipFiles(List<Application.Business.Models.FileNameBody> files)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var fileNameBody in files)
                    {
                        var fileName = archive.CreateEntry(fileNameBody.Name);
                        using (var fileBody = new MemoryStream(fileNameBody.Body))
                        {
                            using (Stream stream = fileName.Open())
                            {
                                fileBody.CopyTo(stream);
                            }
                        }
                    }
                }
                return memoryStream.ToArray();
            }
        }
    }
    
}
