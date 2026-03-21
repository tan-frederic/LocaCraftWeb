using LocaCraftAPI.DTOs.Lease;

namespace LocaCraftAPI.DTOs.Lessor
{
    public static class LessorMapper
    {
        public static Models.Lessor ToEntity(LessorCreateDTO dto)
        {
            return new Models.Lessor
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Address = dto.Address,
                City = dto.City,
                PostalCode = dto.PostalCode,
                Country = dto.Country,
                Phone = dto.Phone,
                Email = dto.Email
            };
        }

        public static LessorResponseDTO ToResponseDTO(Models.Lessor lessor)
        {
            return new LessorResponseDTO
            {
                Id = lessor.Id,
                FirstName = lessor.FirstName,
                LastName = lessor.LastName,
                Address = lessor.Address,
                City = lessor.City,
                PostalCode = lessor.PostalCode,
                Country = lessor.Country,
                Phone = lessor.Phone,
                Email = lessor.Email,
                Leases = lessor.Leases?.Select(lease => new LeaseSummaryDTO
                {
                    Id = lease.Id,
                    LeaseName = lease.LeaseName,
                    MonthlyRent = lease.MonthlyRent,
                    MonthlyCharges = lease.MonthlyCharges,
                    IsOngoing = lease.IsOngoing,
                    Deposit = lease.Deposit,
                    StartDate = lease.StartDate,
                    EndDate = lease.EndDate
                }).ToList() ?? []
            };
        }

        public static void ApplyUpdate(LessorCreateDTO dto, Models.Lessor lessor)
        {
            lessor.FirstName = dto.FirstName;
            lessor.LastName = dto.LastName;
            lessor.Address = dto.Address;
            lessor.City = dto.City;
            lessor.PostalCode = dto.PostalCode;
            lessor.Country = dto.Country;
            lessor.Phone = dto.Phone;
            lessor.Email = dto.Email;
        }

    }
}
