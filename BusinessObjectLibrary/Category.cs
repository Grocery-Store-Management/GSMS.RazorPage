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
        [Required]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
