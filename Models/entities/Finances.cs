using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TTP_Project.Models.entities
{
    public class Finances
    {
        [Key]
        [DisplayName("Transaction ID")]
        public int ID { get; set; }

        [DisplayName("Transaction Name")]
        public string Name { get; set; }

        [DisplayName("From")]
        public string From { get; set; }

        [DisplayName("To")]
        public string To { get; set; }

        [DisplayName("Cost")]
        public decimal Cost { get; set; }

        [DisplayName("Budget")]
        public decimal Budget { get; set; }

        [DisplayName("Date")]
        public DateTime Date { get; set; }

    }
}