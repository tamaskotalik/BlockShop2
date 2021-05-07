using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Microsoft.EntityFrameworkCore;



namespace BlockShop2
{
    public class Controller
    {


        public Controller()
        {
           
        }

        public Product AddOrUpdate(Product product, Price price)
        {           
            if(product !=null && price != null)
            {                
                using (var db = new BlockShopContext())
                {
                    var result = db.Products.Include(p => p.Prices).SingleOrDefault(p => p.ProductId == product.ProductId);

                    if(result != null)
                    {
                        result.Name = product.Name;
                        result.Unit = product.Unit;
                        result.Barcode = product.Barcode != "" ? product.Barcode : result.Barcode;
                        price.PriceId = 0;
                        price.From = DateTime.Now;
                        result.Prices.Add(price);
                        db.SaveChanges();
                        return result;
                    }
                    else
                    {                        
                        product.ProductId = 0;
                        product.Prices.Clear();
                        var result2 = db.Products.Add(product);
                        price.PriceId = 0;
                        price.From = DateTime.Now;
                        result2.Entity.Prices.Add(price);
                        db.SaveChanges();
                        return result2.Entity;
                    }
                }                
            }
            return null;
        }

        public Product GetProductByName(string name)
        {
            Product ret = null;
            using (var db = new BlockShopContext()){
                ret = db.Products.Include(p => p.Prices).First(p => p.Name.Equals(name));
            }
            return ret;           
        }

        public Product GetProductById(int id)
        {
            Product ret = null;
            using (var db = new BlockShopContext())
            {
                ret = db.Products.Include(p => p.Prices).First(p => p.ProductId.Equals(id));
            }
            return ret;
        }
        
        public List<Product> GetProductsbyName(string name)
        {
            List<Product> ret = null;
            using (var db = new BlockShopContext())
            {
                ret = db.Products.Where(p => p.Name.StartsWith(name)).ToList();                
            }
            return ret;
        }

        public List<Product> GetProducts()
        {
            List<Product> ret = null;
            using (var db = new BlockShopContext())
            {
                ret = db.Products.Include(p => p.Prices).ToList();
            }
            return ret;
        }
        public Block SaveBlock(Block block)
        {
            using (var db = new BlockShopContext())
            {
                block.Date = DateTime.Now;
                db.ChangeTracker.TrackGraph(block, node => node.Entry.State = !node.Entry.IsKeySet ? EntityState.Added : EntityState.Unchanged);

                db.SaveChanges();
                return block;
            }
        }
    }
}
