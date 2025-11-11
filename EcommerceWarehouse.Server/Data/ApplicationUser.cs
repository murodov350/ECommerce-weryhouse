using Microsoft.AspNetCore.Identity;

namespace EcommerceWarehouse.Server.Data
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
