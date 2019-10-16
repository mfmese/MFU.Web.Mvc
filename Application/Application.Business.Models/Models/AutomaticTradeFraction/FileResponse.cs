using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Business.Models
{
    public class FileResponse
    {
        public List<FileNameBody> FileNameBodyList { get; set; }
        public string ResponseMessage { get; set; }
        public string ResponseCode { get; set; }

        public FileResponse()
        {
            FileNameBodyList = new List<FileNameBody>();
        }
    }
}
