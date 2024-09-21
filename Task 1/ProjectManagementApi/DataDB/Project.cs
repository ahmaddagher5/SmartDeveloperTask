using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagementApi.DataDB
{
    public class Project
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectId { set; get; }
        public string ProjectTitle { set; get; }
        public string ProjectDescription { set; get; }
        public DateTime StartDate { set; get; }
        public DateTime EndDate { set; get; }
        public string Status { set; get; } = "Active";
        public bool Deleted { set; get; } = false;
        public bool IsValidStatusTransition(string newStatus)
        {
            // Define valid transitions
            var validTransitions = new Dictionary<string, List<string>>
            {
                { "Active", new List<string> { "Completed", "Deferred" } },
                { "Deferred", new List<string> { "Active" } }
            };

            return validTransitions.ContainsKey(Status) && validTransitions[Status].Contains(newStatus);
        }
        public void ValidateStatus()
        {
            var all_status = new string[] { "Active", "Completed", "Deferred" };
            if (string.IsNullOrEmpty(Status) || !all_status.Contains(Status))
            {
                Status = "Active";
            }
        }
        public bool AreDatesValid()
        {
            return StartDate < EndDate;
        }
    }
}
