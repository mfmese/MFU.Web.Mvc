using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Business.Models
{
    public class Mfufile
    {
        [Key]
        public int FileId { get; set; }
        public string FileName { get; set; }
        public DateTime EnterDate { get; set; }
    }
}
