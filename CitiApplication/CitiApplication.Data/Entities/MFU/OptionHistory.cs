using Framework.MakerChecker;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Data.Entities
{
    internal partial class OptionHistory
    {
        [Key]
        public int HistoryId { get; set; }
        public int OptionId { get; set; }
        public string OptionKey { get; set; }
        public string OptionValue { get; set; }

        [Column("State")]
        public int StateId { get; set; }
        public string StateIdName
        {
            get
            {
                return States.GetName(StateId);
            }
        }
        public string StateMaker { get; set; }
        public DateTime StateDate { get; set; }
    }
}
