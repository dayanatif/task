using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSManager.Models
{
    public class DomainDetail
    {
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string DomainName { get; set; } = "";

        [Required, StringLength(150)]
        public string Registrar { get; set; } = "";

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        public DateTime RegisteredDate { get; set; }

        public bool AutoRenew { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        public DateTime RenewalDate { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        [StringLength(200)]
        public string? NameServer1 { get; set; }

        [StringLength(200)]
        public string? NameServer2 { get; set; }
    }
}
