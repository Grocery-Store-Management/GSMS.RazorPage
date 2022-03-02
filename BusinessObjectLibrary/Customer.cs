using System;
using System.Collections.Generic;

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
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Receipt> Receipts { get; set; }
    }
}
