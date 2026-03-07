using System.ComponentModel.DataAnnotations;

namespace TSManager.Models
{
    public class EmailUser
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string FirstName { get; set; } = "";

        [Required, StringLength(100)]
        public string LastName { get; set; } = "";

        [Required, EmailAddress, StringLength(200)]
        public string EmailAddress { get; set; } = "";

        [Required, StringLength(150)]
        public string Company { get; set; } = "";

        [Range(0, 100000)]
        public int StorageGb { get; set; }

        // Later we will secure this (don’t worry now)
        [Required, StringLength(200)]
        public string Password { get; set; } = "";
    }
}
