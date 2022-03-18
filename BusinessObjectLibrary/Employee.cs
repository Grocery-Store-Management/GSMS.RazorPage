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

        [Required(ErrorMessage ="Id is required!")]
        public string Id { get; set; }
        [Required(ErrorMessage = "Name is required!")]
        [StringLength(maximumLength:50, MinimumLength =1, ErrorMessage ="Name must be 1-50 characters!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Password is required!")]
        [StringLength(maximumLength: 50, MinimumLength = 6, ErrorMessage = "Password must be 6-50 characters!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "StoreId is required!")]
        [Display(Name="Store")]
        public string StoreId { get; set; }
        [Required(ErrorMessage = "Role is required!")]
        public string Role { get; set; }
        [Required(ErrorMessage = "Created Date is required!")]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }
        [Required(ErrorMessage = "Is Deleted is required!")]
        [Display(Name = "Status")]
        public bool IsDeleted { get; set; }

        public virtual Store Store { get; set; }
        public virtual ICollection<Receipt> Receipts { get; set; }
    }
}
