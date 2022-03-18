using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class Product
    {
        public Product()
        {
            ImportOrderDetails = new HashSet<ImportOrderDetail>();
            ReceiptDetails = new HashSet<ReceiptDetail>();
        }

        public string Id { get; set; }

        [Display(Name = "Product Name")]
        [Required(ErrorMessage = "Product name is required!!")]
        [RegularExpression(@"^[^@#$%^*()\[\]{}]+$", ErrorMessage = "Product name contains invalid characters!!")]
        [MinLength(6, ErrorMessage = "Product name is required at least 6 characters!!!")]
        [MaxLength(32, ErrorMessage = "Product name is limited to 32 characters!!")]
        public string Name { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "Category is required!!")]
        public string CategoryId { get; set; }

        public bool IsDeleted { get; set; }
        public Status Status { get; set; }

        [Required(ErrorMessage = "Price is required!!")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Price must be a positive number!!")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.0}")]
        public decimal Price { get; set; }

        [Display(Name = "Expired Date")]
        [Required(ErrorMessage = "Expired Date is required!!")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime ExpiringDate { get; set; }

        [Display(Name = "Stored Quantity")]
        [Range(0, int.MaxValue, ErrorMessage = "Stored Quantity must be a positive integer!!")]
        public int StoredQuantity { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<ImportOrderDetail> ImportOrderDetails { get; set; }
        public virtual ICollection<ReceiptDetail> ReceiptDetails { get; set; }
    }
}
