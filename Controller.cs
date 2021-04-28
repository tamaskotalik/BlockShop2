using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace BlockShop2
{
    public class Controller
    {


        public Controller()
        {
           
        }

        public bool AddProduct(Product product, Price price)
        {
            int ret;
            if(product !=null && price != null)
            {
                product.Prices.Add(price);
                using (var db = new BlockShopContext())
                {
                    db.Products.Add(product);
                    ret = db.SaveChanges();
                }
                return ret > 0;
            }
            return false;
        }

    }
}
