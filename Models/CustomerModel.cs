using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace TTP_Project.Models
{
    public class CustomerModel
    {
        [Required]
		[Display(Name = " Ім'я користувача")]
		[StringLength(50)]
		public string UserName { get; set; }

		[Required]
		[Display(Name = "Адреса електороної пошти")]
		[RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$",
ErrorMessage = "Неправильний формат електоронної адреси")]
		public string Email { get; set; }

		[Required]  
		[StringLength(250)]
		[Display(Name = "Примітка")]
		public string MessageText { get; set; }
	}

    }
