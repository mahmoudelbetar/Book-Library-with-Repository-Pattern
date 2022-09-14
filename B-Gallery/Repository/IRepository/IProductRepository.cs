﻿using B_Gallery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_Gallery.DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> FilterProductByCategory(string? category = null);
        void Update(Product product);
        void Save();
    }
}
