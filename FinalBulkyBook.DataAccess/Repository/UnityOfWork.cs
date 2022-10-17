﻿using FinalBulkyBook.DataAccess.Repository.IRepository;
using FinalBulkyBook.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalBulkyBook.DataAccess.Repository
{
    public class UnityOfWork : IUnityOfWork
    {
        private ApplicationDbContext _db;

        public UnityOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            CoverType = new CoverTypeRepository(_db);
            Product = new ProductRepository(_db);
        }

        public ICategoryRepository Category { get; private set; }
        public ICoverTypeRepository CoverType { get; private set; }
        public IProductRepository Product { get; private set; }


        public void Save()
        {
            _db.SaveChanges();
        }
    }
}