using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class ImportOrder
    {
        public ImportOrder()
        {
            ImportOrderDetails = new HashSet<ImportOrderDetail>();
        }

        public string Id { get; set; }

        [Display(Name = "Import Order Name")]
        [Required(ErrorMessage = "Import Order name must not be empty")]
        [RegularExpression(@"^[^@#$%^*()\[\]{}]+$", ErrorMessage = "Import Order must not contain special characters")]
        [MinLength(6, ErrorMessage = "Import Order name must be between 6-32 characters")]
        [MaxLength(32, ErrorMessage = "Import Order name must be between 6-32 characters")]
        public string Name { get; set; }

        [Display(Name = "Store")]
        [Required(ErrorMessage = "Store must not be empty")]
        public string StoreId { get; set; }
        public bool IsDeleted { get; set; }

        [Display(Name = "Imported Date")]
        [Required(ErrorMessage = "Imported Date must not be empty")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime CreatedDate { get; set; }

        public virtual Store Store { get; set; }
        public virtual ICollection<ImportOrderDetail> ImportOrderDetails { get; set; }
    }
}
