using FinalBulkyBook.DataAccess.Repository.IRepository;
using FinalBulkyBook.DataAccess.Data;
using FinalBulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalBulkyBook.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoopingCart>, IShoppingCartRepository
    {
        private ApplicationDbContext _db;

        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public int DecrementCount(ShoopingCart shoopingCart, int count)
        {
            shoopingCart.Count -= count;
            return shoopingCart.Count;
        }

        public int IncrementCount(ShoopingCart shoopingCart, int count)
        {
            shoopingCart.Count += count;
            return shoopingCart.Count;
        }
    }
}
