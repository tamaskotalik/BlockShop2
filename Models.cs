using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Models
{
    public class BlockShopContext : DbContext
    {
        public DbSet<Price> Price { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<BlockItem> BlockItems { get; set; }

        public DbSet<Block> Blocks { get; set; }

        // The following configures EF to create a Sqlite database file as `C:\blogging.db`.
        // For Mac or Linux, change this to `/tmp/blogging.db` or any other absolute path.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(@"Data Source=C:\test\BlockShopDB2.db");
    }

    public class Price
    {
        public int PriceId { get; set; }

        public long price { get; set; }

        public long tax { get; set; }

        public DateTime From { get; set; }

        public DateTime to { get; set; }

//        public List<Post> Posts { get; } = new List<Post>();
    }

    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public string Unit { get; set; }

        public List<Price> Prices { get; } = new List<Price>();

        public override string ToString()
        {
            return Name;
        }

    }
    /// <summary>
    /// 
    /// </summary>

    public class BlockItem
    {
        public int BlockItemID { get; set; }
        public Product Product { get; set; }

        public double Volume { get; set; }

    }

    public class Block
    {
        public int BlockID { get; set; }
        public DateTime Date { get; set; }
        public List<BlockItem> BlockItems { get; } = new List<BlockItem>();

        public double Szum;

        public double SzumTax;
    }
}