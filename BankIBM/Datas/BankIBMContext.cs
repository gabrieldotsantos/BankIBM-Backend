using BankIBM.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankIBM.Datas
{
    public class BankIBMContext : DbContext
    {
        public BankIBMContext(DbContextOptions<BankIBMContext> options) : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Transaction> Transactions { get; set; }
    }
}
