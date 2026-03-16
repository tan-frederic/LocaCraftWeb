namespace LocaCraftAPI.DTOs.RealEstateAsset
{
    public static class RealEstateAssetMapper
    {
        public static Models.RealEstateAsset ToEntity(RealEstateAssetCreateDTO dto)
        {
            return new Models.RealEstateAsset
            {
                Name = dto.Name,
                Description = dto.Description,
                Address = dto.Address,
                AddressComplement = dto.AddressComplement,
                PostalCode = dto.PostalCode,
                City = dto.City,
                Country = dto.Country,
                UserId = dto.UserId
            };
        }

        public static RealEstateAssetResponseDTO ToResponseDTO(Models.RealEstateAsset asset)
        {
            return new RealEstateAssetResponseDTO
            {
                Id = asset.Id,
                Name = asset.Name,
                Description = asset.Description,
                Address = asset.Address,
                AddressComplement = asset.AddressComplement,
                PostalCode = asset.PostalCode,
                City = asset.City,
                Country = asset.Country,
                Leases = asset.Leases.Select(lease => new LeaseSummaryDTO
                {
                    Id = lease.Id,
                    LeaseName = lease.LeaseName,
                    MonthlyRent = lease.MonthlyRent,
                    MonthlyCharges = lease.MonthlyCharges,
                    Deposit = lease.Deposit,
                    IsOngoing = lease.IsOngoing,
                    StartDate = lease.StartDate,
                    EndDate = lease.EndDate
                }).ToList()
            };
        }

        public static void ApplyUpdate(RealEstateAssetCreateDTO dto, Models.RealEstateAsset asset)
        {
            asset.Name = dto.Name;
            asset.Description = dto.Description;
            asset.Address = dto.Address;
            asset.AddressComplement = dto.AddressComplement;
            asset.PostalCode = dto.PostalCode;
            asset.City = dto.City;
            asset.Country = dto.Country;
            asset.UserId = dto.UserId;
        }
    }
}
