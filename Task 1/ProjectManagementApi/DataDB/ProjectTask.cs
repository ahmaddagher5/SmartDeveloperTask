using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApi.DataDB
{
    public class ProjectTask
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskId { get; set; }
        public string TaskTitle { get; set; }
        public DateTime StartDate { set; get; }
        public DateTime EndDate { set; get; }
        public string Priority { get; set; } = "Low";
        public string Status { get; set; } = "In Progress";
        public int ProjectId { set; get; }
        public int TaskOwnerId { get; set; }
        public bool IsRecurring { get; set; } = false;
        public int? RecurrencePatternId { get; set; }

        public void ValidatePriority()
        {
            var all_status = new string[] { "Low", "Medium", "High" };
            if (string.IsNullOrEmpty(Priority) || !all_status.Contains(Priority))
            {
                Priority = "Low";
            }
        }
        public void ValidateStatus()
        {
            var all_status = new string[] { "In Progress", "Completed", "Deferred" };
            if (string.IsNullOrEmpty(Priority) || !all_status.Contains(Priority))
            {
                Status = "In Progress";
            }
        }
        public bool IsValidStatusTransition(string newStatus)
        {
            // Define valid transitions
            var validTransitions = new Dictionary<string, List<string>>
            {
                { "In Progress", new List<string> { "Completed", "Deferred" } },
                { "Deferred", new List<string> { "In Progress" } }
            };

            return validTransitions.ContainsKey(Status) && validTransitions[Status].Contains(newStatus);
        }
        public bool AreDatesValid()
        {
            return StartDate < EndDate;
        }
    }
}
