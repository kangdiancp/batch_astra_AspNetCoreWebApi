using Northwind.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Entities.DataTransferObject
{
    public abstract class CustomerForManipulationDto
    {
        [Required(ErrorMessage = "CustomerId is required")]
        [MaxLength(5, ErrorMessage = "Maximum length for customerId is 5 characters")]
        public string CustomerId { get; set; }

        [Required(ErrorMessage = "CompanyName is required")]
        [MaxLength(40, ErrorMessage = "Maximum length for customerName is 40 characters")]
        public string CompanyName { get; set; }
        public string ContactName { get; set; }

        public ICollection<Order> Orders { get; set; }


    }
}
