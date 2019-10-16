using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Business.Models
{
    public static class FileState
    {
        public static IdName Processed = new IdName { Id = 1, Name = "Processed" };
        public static IdName UnProcessed = new IdName { Id = 0, Name = "UnProcessed" };
    }
}
