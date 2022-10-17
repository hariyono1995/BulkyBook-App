﻿using FinalBulkyBook.Models;
using FinalBulkyBook.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalBulkyBook.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<CoverType> CoverTypes { get; set; }
        public DbSet<Product> Products { get; set; }

    }
}