using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace B_Gallery.Models.ViewModels
{
    public class ShoppingCart
    {
        public int Count { get; set; }
        public Product Product { get; set; }
    }
}
