using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class Employee
    {
        public Employee()
        {
            Receipts = new HashSet<Receipt>();
        }

        [Required(ErrorMessage ="Id must not be empty!")]
        public string Id { get; set; }
        [Required(ErrorMessage = "Name must not be empty!")]
        [StringLength(maximumLength:50, MinimumLength =1, ErrorMessage ="Name must be between 1-50 characters!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Password must not be empty!")]
        [StringLength(maximumLength: 50, MinimumLength = 6, ErrorMessage = "Password must be between 6-50 characters!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "StoreId must not be empty!")]
        [Display(Name="Store")]
        public string StoreId { get; set; }
        [Required(ErrorMessage = "Role must not be empty!")]
        public string Role { get; set; }
        [Required(ErrorMessage = "Created Date must not be empty!")]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }
        [Required(ErrorMessage = "Is Deleted must not be empty!")]
        [Display(Name = "Status")]
        public bool IsDeleted { get; set; }

        public virtual Store Store { get; set; }
        public virtual ICollection<Receipt> Receipts { get; set; }
    }
}
