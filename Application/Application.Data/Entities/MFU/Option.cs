using Framework.MakerChecker;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Data.Entities
{
    internal partial class Option: MakerCheckerEntity
    {
        public int OptionId { get; set; }
        public string OptionKey { get; set; }
        public string OptionValue { get; set; }

        [Column("State")]
        public new int StateId { get; set; }

        public new string StateIdName
        {
            get
            {
                return States.GetName(StateId);
            }
        }
    }
}
