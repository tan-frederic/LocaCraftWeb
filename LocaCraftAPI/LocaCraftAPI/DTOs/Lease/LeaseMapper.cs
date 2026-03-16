namespace LocaCraftAPI.DTOs.Lease
{
    public static class LeaseMapper
    {
        public static Models.Lease ToEntity(LeaseCreateDTO dto)
        {
            return new Models.Lease
            {
                RealEstateAssetId = dto.RealEstateAssetId,
                LessorId = dto.LessorId,
                LeaseName = dto.LeaseName,
                MonthlyRent = dto.MonthlyRent,
                MonthlyCharges = dto.MonthlyCharges,
                Deposit = dto.Deposit,
                RentIndexReference = dto.RentIndexReference,
                IsOngoing = dto.IsOngoing,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };
        }

        public static LeaseResponseDTO ToResponseDTO(Models.Lease lease)
        {
            return new LeaseResponseDTO
            {
                Id = lease.Id,
                RealEstateAssetId = lease.RealEstateAssetId,
                RealEstateAssetName = lease.RealEstateAsset?.Name ?? string.Empty,
                LessorId = lease.LessorId,
                LessorFirstName = lease.Lessor?.FirstName ?? string.Empty,
                LessorLastName = lease.Lessor?.LastName ?? string.Empty,
                LeaseName = lease.LeaseName,
                MonthlyRent = lease.MonthlyRent,
                MonthlyCharges = lease.MonthlyCharges,
                Deposit = lease.Deposit,
                RentIndexReference = lease.RentIndexReference,
                IsOngoing = lease.IsOngoing,
                StartDate = lease.StartDate,
                EndDate = lease.EndDate,
                Tenants = lease.Tenants?.Select(t => new TenantSummaryDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Surname = t.Surname,
                    Email = t.Email,
                    PhoneNumber = t.PhoneNumber
                }).ToList() ?? []
            };
        }
        
        public static void ApplyUpdate(LeaseCreateDTO dto, Models.Lease lease)
        {
            lease.RealEstateAssetId = dto.RealEstateAssetId;
            lease.LessorId = dto.LessorId;
            lease.LeaseName = dto.LeaseName;
            lease.MonthlyRent = dto.MonthlyRent;
            lease.MonthlyCharges = dto.MonthlyCharges;
            lease.Deposit = dto.Deposit;
            lease.RentIndexReference = dto.RentIndexReference;
            lease.IsOngoing = dto.IsOngoing;
            lease.StartDate = dto.StartDate;
            lease.EndDate = dto.EndDate;
        }
    }

}
