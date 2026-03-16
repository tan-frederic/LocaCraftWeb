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
                Leases = lessor.Leases.Select(l => new LeaseSummaryDto
                {
                    Id = l.Id,
                    LeaseName = l.LeaseName,
                    MonthlyRent = l.MonthlyRent,
                    MonthlyCharges = l.MonthlyCharges,
                    Deposit = l.Deposit,
                    StartDate = l.StartDate,
                    EndDate = l.EndDate
                }).ToList()
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
