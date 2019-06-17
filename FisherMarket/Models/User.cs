using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace FisherMarket.Models
{
    public class User : IdentityUser<int>
    {
        public int RandomConfirm { get; set; }
        public string EmailConfirmToken { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
