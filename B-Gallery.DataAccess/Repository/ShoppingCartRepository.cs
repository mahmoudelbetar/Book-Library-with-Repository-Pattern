using B_Gallery.DataAccess.Repository.IRepository;
using B_Gallery.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace B_Gallery.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly ApplicationDbContext _db;
       
        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
            
        }

        public int Decreament(ShoppingCart cart, int count)
        {
            return cart.Count -= count;
        }


        public int Increament(ShoppingCart cart, int count)
        {
            return cart.Count += count;
        }
    }
}
