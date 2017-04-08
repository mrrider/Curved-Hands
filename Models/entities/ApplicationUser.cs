using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace TTP_Project.Models.entities
{
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public ICollection<ApplicationUserRole> UserRoles { get; set; }

        [Required]
        public string FistName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Organization { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
         [Required]
        public string RoleName { get; set; }

        public virtual ICollection<Order> orders { get; set; }
    }
}