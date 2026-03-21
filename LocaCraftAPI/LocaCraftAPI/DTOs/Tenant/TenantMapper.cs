namespace LocaCraftAPI.DTOs.Tenant
{
    public static class TenantMapper
    {
        public static Models.Tenant ToEntity(TenantCreateDTO dto)
        {
            return new Models.Tenant
            {
                LeaseId = dto.LeaseId,
                Name = dto.Name,
                Surname = dto.Surname,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                Address = dto.Address,
                City = dto.City,
                PostalCode = dto.PostalCode,
                Country = dto.Country
            };
        }

        public static TenantResponseDTO ToResponseDTO(Models.Tenant tenant)
        {
            return new TenantResponseDTO
            {
                Id = tenant.Id,
                LeaseId = tenant.LeaseId,
                Name = tenant.Name,
                Surname = tenant.Surname,
                PhoneNumber = tenant.PhoneNumber,
                Email = tenant.Email,
                Address = tenant.Address,
                City = tenant.City,
                PostalCode = tenant.PostalCode,
                Country = tenant.Country
            };
        }

        public static void ApplyUpdate(TenantCreateDTO dto, Models.Tenant tenant)
        {
            tenant.LeaseId = dto.LeaseId;
            tenant.Name = dto.Name;
            tenant.Surname = dto.Surname;
            tenant.PhoneNumber = dto.PhoneNumber;
            tenant.Email = dto.Email;
            tenant.Address = dto.Address;
            tenant.City = dto.City;
            tenant.PostalCode = dto.PostalCode;
            tenant.Country = dto.Country;
        }
    }
}
