using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TTP_Project.Models.constants;

namespace TTP_Project.Models.entities
{
    [Bind(Exclude = "ID")]
    public class ProductItem
    {
        [Key]
        [ScaffoldColumn(false)]
        public int ID {get; set;}

        [Required(ErrorMessage = "An Item Name is required")]
        [StringLength(160)]
        public string Name {get; set;}

        public String shortDescription {get; set;}
        public String description {get; set;}
        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 999999.99, ErrorMessage = "Price must be between 0.01 and 999999.99")]
        public decimal Price { get; set;}
        public String toString()
        {
            return Name + " " + Price + " " +description;
        }

        public byte[] InternalImage { get; set; }

        [Display(Name = "Local file")]
        [NotMapped]
        public HttpPostedFileBase File
        {
            get
            {
                return null;
            }

            set
            {
                try
                {
                    MemoryStream target = new MemoryStream();

                    if (value.InputStream == null)
                        return;

                    value.InputStream.CopyTo(target);
                    InternalImage = target.ToArray();
                }
                catch (Exception ex)
                {
                    //logger.Error(ex.Message);
                    // logger.Error(ex.StackTrace);
                }
            }
        }

        [DisplayName("Item Picture URL")]
        [StringLength(1024)]
        public string ItemPictureUrl { get; set; }
        

        public TemplateSiteTypes Categorie { get; set; }

    }

}