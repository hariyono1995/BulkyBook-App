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
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private ApplicationDbContext _db;

        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Company obj)
        {
            _db.Companies.Update(obj);
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
