﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TTP_Project.Models.entities;

namespace TTP_Project.Models.ViewModels
{
    public class RoleViewModel
    {
        public string RoleName { get; set; }
        public string Description { get; set; }

        public RoleViewModel() { }
        public RoleViewModel(ApplicationRole role)
        {
            this.RoleName = role.Name;
            this.Description = role.Description;
        }
    }

    public class SelectRoleEditorViewModel
    {
        public SelectRoleEditorViewModel() { }
        
        public SelectRoleEditorViewModel(ApplicationRole role)
        {
            this.RoleName = role.Name;
            
            this.Description = role.Description;
        }

        [Required]
        public string RoleName { get; set; }
        
        public string Description { get; set; }
    }

    public class EditRoleViewModel
    {
        public string OriginalRoleName { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }

        public EditRoleViewModel() { }
        public EditRoleViewModel(ApplicationRole role)
        {
            this.OriginalRoleName = role.Name;
            this.RoleName = role.Name;
            this.Description = role.Description;
        }
    }
}