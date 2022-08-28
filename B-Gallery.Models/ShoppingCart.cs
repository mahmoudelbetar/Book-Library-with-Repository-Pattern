using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_Gallery.Models
{
    public class ShoppingCart
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int Count { get; set; }
        [ForeignKey("Product")]
        public int? ProductId { get; set; }
        [ValidateNever]
        public virtual Product? Product { get; set; }
        [ForeignKey("ApplicationUser")]
        public string? ApplicationUserId { get; set; } = Guid.NewGuid().ToString();
        [ValidateNever]
        public virtual ApplicationUser? ApplicationUser { get; set; }

    }
}
