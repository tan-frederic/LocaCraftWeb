using System.ComponentModel.DataAnnotations;

namespace LocaCraftAPI.Models
{
    public class Tenant
    {
        public int Id { get; set; }
        public int LeaseId { get; set; }
        public Lease? Lease { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Address { get; set; } = string.Empty;
        [Required]
        public string City { get; set; }= string.Empty;
        [Required]
        public string PostalCode { get; set; } = string.Empty;
        [Required]
        public string Country { get; set; } = string.Empty;
    }
}
