using System.ComponentModel.DataAnnotations;

namespace LocaCraftAPI.DTOs.RealEstateAsset
{
    public class RealEstateAssetCreateDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Address { get; set; } = string.Empty;
        public string AddressComplement { get; set; } = string.Empty;
        [Required]
        public string PostalCode { get; set; } = string.Empty;
        [Required]
        public string City { get; set; } = string.Empty;
        [Required]
        public string Country { get; set; } = string.Empty;
    }
}
