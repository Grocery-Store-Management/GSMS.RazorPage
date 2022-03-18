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
        [Required(ErrorMessage = "Import Order name is required!!")]
        [RegularExpression(@"^[^@#$%^*()\[\]{}]+$", ErrorMessage = "Import Order name contains invalid characters!!")]
        [MinLength(6, ErrorMessage = "Import Order name is required at least 6 characters!!!")]
        [MaxLength(32, ErrorMessage = "Import Order name is limited to 32 characters!!")]
        public string Name { get; set; }

        [Display(Name = "Store")]
        [Required(ErrorMessage = "Store is required!!")]
        public string StoreId { get; set; }
        public bool IsDeleted { get; set; }

        [Display(Name = "Imported Date")]
        [Required(ErrorMessage = "Imported Date is required!!")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime CreatedDate { get; set; }

        public virtual Store Store { get; set; }
        public virtual ICollection<ImportOrderDetail> ImportOrderDetails { get; set; }
    }
}
