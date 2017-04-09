using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TTP_Project.Models.entities;

namespace TTP_Project.Models.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        [Required]
        public string FistName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Organization { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        [Required]
        public string Role { get; set; }


        public EditUserViewModel() { }
        public EditUserViewModel(ApplicationUser user)
        {

            //this.UserName = user.UserName;
            // this.Email = user.Email;
            this.FistName = user.FistName;
            this.LastName = user.LastName;
            this.Organization = user.Organization;
            this.City = user.City;
            this.Country = user.Country;
            this.Role = user.RoleName;
            
        }
    }
}