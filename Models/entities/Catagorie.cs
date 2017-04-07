using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTP_Project.Models.entities
{
    public class Catagorie
    {
        [Key]
        [DisplayName("Catagorie ID")]
        public int ID { get; set; }

        [DisplayName("Catagorie")]
        public string Name { get; set; }

        public virtual ICollection<ProductItem> Items { get; set; }
    }
}