using Microsoft.AspNetCore.Identity;

namespace OdataSolution.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsBanned { get; set; }

        public DateTime? TimeOfBanning { get; set; }

        public string? Reason { get; set; }
    }
}
