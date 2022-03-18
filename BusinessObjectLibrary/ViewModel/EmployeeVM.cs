using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjectLibrary.ViewModel
{
    public class EmployeeVM
    {
        [Required(ErrorMessage = "Name is required!")]
        [StringLength(maximumLength: 50, MinimumLength = 1, ErrorMessage = "Name must be 1-50 characters!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Password is required!")]
        [StringLength(maximumLength: 50, MinimumLength = 6, ErrorMessage = "Password must be 6-50 characters!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "StoreId is required!")]
        [Display(Name = "Store")]
        public string StoreId { get; set; }
        [Required(ErrorMessage = "Role is required!")]
        public string Role { get; set; }

        [Display(Name = "Status")]
        public bool IsDeleted { get; set; }
    }
}
