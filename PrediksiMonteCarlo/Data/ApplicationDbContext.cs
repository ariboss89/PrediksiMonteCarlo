using System;
using Microsoft.EntityFrameworkCore;
using PrediksiMonteCarlo.Models;

namespace PrediksiMonteCarlo.Data
{
	public class ApplicationDbContext :DbContext
	{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Merk> Merks { get; set; }

        public DbSet<Motor> Motors { get; set; }

        public DbSet<Penjualan> Penjualans { get; set; }

        public DbSet<Pengguna> Penggunas { get; set; }
    }
}

