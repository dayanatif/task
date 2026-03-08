using System.ComponentModel.DataAnnotations;

namespace TSManager.Models
{
    public class NotificationLog
    {
        public int Id { get; set; }

        [Required]
        public string ItemType { get; set; } = ""; // "Domain" or "Software"

        public int ItemId { get; set; }

        public int DaysBefore { get; set; } // 30 / 14 / 7

        public DateTime SentOnDate { get; set; } // store "today" when sent
    }
}
