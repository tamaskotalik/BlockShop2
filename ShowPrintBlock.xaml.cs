using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BlockShop2
{
    /// <summary>
    /// Interaction logic for ShowPrintBlock.xaml
    /// </summary>
    public partial class ShowPrintBlock : Page
    {
        public event EventHandler OnFinishedPrint;
        public ShowPrintBlock()
        {
            InitializeComponent();            
        }

        public ShowPrintBlock(Models.Block actblock)
        {
            InitializeComponent(); 
            string text = LoadHeader();
            text += "<table style=\"width: 100 % \">";

            double summ = 0;

            if (actblock != null)
            {
                foreach (var item in actblock.BlockItems)
                {
                    text += "<tr>";
                    summ += (item.Product.LastPrice.price * item.Volume);
                    text += "<td>" + item.Product.Name + "</td><td>" + item.Volume + item.Product.Unit + "</td><td>" + (item.Product.LastPrice.price * item.Volume).ToString() + " Ft " + "</td>";                    
                    text += "</tr>";
                }
            }
            text += "<tr>";
            text += "<td> Összesen " + "</td><td>" + "</td><td>"+ summ +  " Ft " + "</td>";
            text += "</tr>";
            text += "</table>";
            text += "A számla sorszáma: " + actblock.BlockID;
            text += "<br>Dátum: " + DateTime.Now.ToString();
            text += LoadFooter();
            Browser.Width = 400;
            Browser.NavigateToString(text);
        }

        public string LoadHeader()
        {
            return System.IO.File.ReadAllText("header.html");
        }

        public string LoadFooter()
        {
            return System.IO.File.ReadAllText("footer.html");
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            Browser.Focus();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            OnFinishedPrint?.Invoke(this,e);
        }
    }
}
