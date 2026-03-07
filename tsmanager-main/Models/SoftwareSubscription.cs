using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSManager.Models
{
    public class SoftwareSubscription
    {
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Software { get; set; } = "";

        [Required, StringLength(100)]
        public string Category { get; set; } = "";

        [Required, StringLength(20)]
        public string PlanType { get; set; } = "";

        [Column(TypeName = "decimal(10,2)")]
        [Range(0, 999999999)]
        public decimal Amount { get; set; }

        [Required, EmailAddress, StringLength(200)]
        public string SubscribedEmail { get; set; } = "";

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        public DateTime SubscribedDate { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        public DateTime RenewalDate { get; set; }

    }
}
