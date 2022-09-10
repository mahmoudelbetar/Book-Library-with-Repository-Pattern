using B_Gallery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace B_Gallery.DataAccess.Repository.IRepository
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        int Increament(ShoppingCart cart, int count);
        int Decreament(ShoppingCart cart, int count);
        IEnumerable<ShoppingCart> GetAll(Expression<Func<ShoppingCart, bool>> filter, string? includeProperties = null);
        void Update(ShoppingCart shoppingCart);
        int CountCart();
    }
}
