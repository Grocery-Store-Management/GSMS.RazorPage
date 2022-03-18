#nullable disable

using System.ComponentModel.DataAnnotations;

namespace BusinessObjectLibrary
{
    public partial class ImportOrderDetail
    {
        public string Id { get; set; }
        public string OrderId { get; set; }

        [Display(Name = "Product Name")]
        [Required(ErrorMessage = "Product name must not be empty")]
        [RegularExpression(@"^[^@#$%^*()\[\]{}]+$", ErrorMessage = "Product name contains invalid characters!!")]
        [MinLength(6, ErrorMessage = "Product name is required at least 6 characters!!!")]
        [MaxLength(32, ErrorMessage = "Product name is limited to 32 characters!!")]
        public string Name { get; set; }

        [Display(Name = "Distributor")]
        [Required(ErrorMessage = "Distributor must not be empty")]
        //[RegularExpression(@"^[^@#$%^*()\[\]{}]+$", ErrorMessage = "Distributor contains invalid characters!!")]
        //[MinLength(6, ErrorMessage = "Distributor is required at least 6 characters!!!")]
        //[MaxLength(32, ErrorMessage = "Distributor is limited to 32 characters!!")]
        public string Distributor { get; set; }
        public string ProductId { get; set; }

        [Display(Name = "Quantity")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a positive integer!!")]
        public int Quantity { get; set; }
        public bool IsDeleted { get; set; }

        [Required(ErrorMessage = "Price must not be empty")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Price must be a positive number!!")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.0}")]
        public decimal Price { get; set; }

        public virtual ImportOrder Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
