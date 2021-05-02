using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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
            => options.UseSqlite(@"Data Source=BlockShopDB3.db");
    }

    public class Price
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PriceId { get; set; }

        public long price { get; set; }

        public double taxrate { get; set; }

        public DateTime From { get; set; }

//        public DateTime to { get; set; }

//        public List<Post> Posts { get; } = new List<Post>();
    }

    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public string Unit { get; set; }

        public List<Price> Prices { get; } = new List<Price>();
        public List<BlockItem> BlockItems { get; } = new List<BlockItem>();

        public Price LastPrice
        {
            get
            {
                return Prices.Last();
            }    
        }

        public string LastPriceAndUnit
        {
            get
            {
                return Prices.Last().price.ToString() + " Ft/" + Unit;
            }
        }


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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BlockItemID { get; set; }
        public Product Product { get; set; }

        public double Volume { get; set; }

        public string VolumeToString
        {
            get
            {
                return Volume.ToString("F2") + " " + Product.Unit;
            }
        }

    }

    public class Block
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BlockID { get; set; }
        public DateTime Date { get; set; }
        public List<BlockItem> BlockItems { get; } = new List<BlockItem>();

        public double Szum;

        public double SzumTax;
    }
}