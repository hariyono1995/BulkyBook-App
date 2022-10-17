using FinalBulkyBook.DataAccess.Repository.IRepository;
using FinalBulkyBook.Data;
using FinalBulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalBulkyBook.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product obj)
        {
            var firstFromDb = _db.Products.FirstOrDefault(p => p.Id == obj.Id);

            if (firstFromDb != null)
            {
                firstFromDb.Title = obj.Title;
                firstFromDb.Description = obj.Description;
                firstFromDb.ISBN = obj.ISBN;
                firstFromDb.Author = obj.Author;
                firstFromDb.ListPrice = obj.ListPrice;
                firstFromDb.Price = obj.Price;
                firstFromDb.Price50 = obj.Price50;
                firstFromDb.Price100 = obj.Price100;
                firstFromDb.CategoryId = obj.CategoryId;
                firstFromDb.CoverTypeId = obj.CoverTypeId;
                if (obj.ImageUrl != null)
                    firstFromDb.ImageUrl = obj.ImageUrl;
            }

                _db.Products.Update(firstFromDb);
        }

        //void ICategoryRepository.Save()
        //{
        //    _db.SaveChanges();
        //}

        //void ICategoryRepository.Update(Category obj)
        //{
        //    _db.Categories.Update(obj);
        //}
    }
}
