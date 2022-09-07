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

        public int CountCart()
        {
            return _db.ShoppingCarts.Sum(c => c.Count);
        }

        public int Decreament(ShoppingCart cart, int count)
        {
            return cart.Count -= count;
        }


        public int Increament(ShoppingCart cart, int count)
        {
            return cart.Count += count;
        }

        public void Update(ShoppingCart shoppingCart)
        {
            _db.ShoppingCarts.Update(shoppingCart);
        }
    }
}
