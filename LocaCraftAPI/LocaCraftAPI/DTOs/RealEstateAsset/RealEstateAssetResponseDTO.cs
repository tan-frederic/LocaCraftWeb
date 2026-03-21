using LocaCraftAPI.DTOs.Lease;

namespace LocaCraftAPI.DTOs.RealEstateAsset
{
    public class RealEstateAssetResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string AddressComplement { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public List<LeaseSummaryDTO> Leases { get; set; } = new List<LeaseSummaryDTO>();
    }

}
