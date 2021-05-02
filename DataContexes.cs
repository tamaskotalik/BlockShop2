using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Models;

namespace BlockShop2
{
    public class ProductDataContext
    {
        public Product product { get; set; }
        public Price price { get; set; }

    }

    public class BlockSumms
    {
        public int SumPrice { get; set; }
        public int SumTax { get; set; }
    }

}
