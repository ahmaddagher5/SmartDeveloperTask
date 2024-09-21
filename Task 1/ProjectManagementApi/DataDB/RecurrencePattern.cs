using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApi.DataDB
{
    public class RecurrencePattern
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecurrencePatternId { get; set; }
        public string Pattern { get; set; } // Daily, Weekly, Monthly, etc.
        public int DaysCount { get; set; }
    }
}
