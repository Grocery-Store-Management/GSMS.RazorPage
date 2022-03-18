using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class Customer
    {
        public Customer()
        {
            Receipts = new HashSet<Receipt>();
        }

        public string Id { get; set; }
        public int Point { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone number")]
        [RegularExpression("[0-9]{8,10}", ErrorMessage ="Valid phone numbers are from 8 to 10 numbers only")]
        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Receipt> Receipts { get; set; }
    }
}
