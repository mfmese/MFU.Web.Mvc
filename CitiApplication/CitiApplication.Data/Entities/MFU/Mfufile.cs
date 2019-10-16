using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Data.Entities
{
    internal partial class Mfufile
    {
        [Key]
        public int FileId { get; set; }
        public string FileName { get; set; }
        public DateTime EnterDate { get; set; }

        [ForeignKey("FileId")]
        public ICollection<Mfumodel> MfumodelList { get; set; }
    }
}
