using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }
        [Required]
        public string Id { get; set; }

        [Display(Name = "Category Name")]
        [Required(ErrorMessage = "Category name is required!!")]
        [RegularExpression(@"^[^@#$%^*()\[\]{}]+$", ErrorMessage = "Category name contains invalid characters!!")]
        [MinLength(6, ErrorMessage = "Category name is required at least 6 characters!!!")]
        [MaxLength(32, ErrorMessage = "Category name is limited to 32 characters!!")]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
