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
        [Required(ErrorMessage = "Category name must not be empty")]
        [RegularExpression(@"^[^@#$%^*()\[\]{}]+$", ErrorMessage = "Category name must not contain special characters!!!")]
        [MinLength(6, ErrorMessage = "Category name must be between 6 to 32 characters in length")]
        [MaxLength(32, ErrorMessage = "Category name must be between 6 to 32 characters in length")]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
