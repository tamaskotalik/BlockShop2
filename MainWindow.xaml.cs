using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        NewProduct NewProductPage;
        Controller Controller;

        Models.Block Block;
        BlockSumms BlockSumms;
        public MainWindow()
        {
            InitializeComponent();

            var db = new  Models.BlockShopContext();
            db.CreateDB();

            Controller = new Controller();

            Block = new Models.Block();
            CurrentBlock.DataContext = Block;

            BuildProductButtonList();

            BlockSumms = new BlockSumms();
            CurrentBlockSummary.DataContext = BlockSumms;

            NewProductPage = new NewProduct();
            NewProductPage.OnFinishedEnty += new EventHandler(NewProductPage_OnFinishedEnty);
            Frame.Visibility = Visibility.Collapsed;
            ShowPage(NewProductPage);
        }

        /// <summary>
        /// Termék gombok létrehozása és panelhez adása
        /// </summary>
        private void BuildProductButtonList()
        {
            BuildProductButtonList("", "");
        }

        /// <summary>
        /// Termék gombok létrehozása és panelhez adása
        /// </summary>
        /// <param name="name">Név paraméter ha nem üres akkor ez alapján rendezve és szűrve kerülnek a gombok a panelre</param>
        /// <param name="barcode">Vonalkód paraméter ha nem üres akkor ez alapján kerülnek rendezve és szűrve a gombok a panelre</param>
        private void BuildProductButtonList(string name, string barcode)
        {

            var ProductList = Controller.GetProducts();
            if (name != "")
            {
                var Matches = from p in ProductList
                              where p.Name.StartsWith(name)
                              orderby p.Name
                              select p;
                ProductList = Matches.ToList();
            }

            if(barcode.Length > 0)
            {
                var Matches = from p in ProductList
                              where p.Barcode == barcode
                              orderby p.Name
                              select p;
                ProductList = Matches.ToList();
            }
            
            ProductBtnList.Children.Clear();
            foreach (var item in ProductList)
            {
                var PDC = new ProductDataContext();
                PDC.product = item;
                PDC.price = item.Prices.Last();
                var btn = new DataBtn(PDC);
                btn.OnClickSelected += new EventHandler(ProductBtnClicked);
                ProductBtnList.Children.Add(btn);
            }

        }

        /// <summary>
        /// Termék gombra kattintás eseménykezelő
        /// </summary>
        /// <param name="sender">A termék adatai</param>
        /// <param name="e"></param>
        private void ProductBtnClicked(object sender, EventArgs e)
        {
            if (VolumeTextBox.Text != "")
            {
                var tmpBlockItem = new Models.BlockItem();

                tmpBlockItem.Product = ((ProductDataContext)sender).product;

                tmpBlockItem.Volume = double.Parse(VolumeTextBox.Text);

                Block.BlockItems.Add(tmpBlockItem);

                RecalculateSumms();

                UpdateList();

                VolumeTextBox.Text = "1";
                ProductNameORBarcode.Text = "";
            }
        }

        /// <summary>
        /// A végösszeg és az össesített adó kiszámítása
        /// </summary>
        private void RecalculateSumms()
        {
            var tmpBlockSumm = new BlockSumms();

            foreach (var item in Block.BlockItems)
            {
                tmpBlockSumm.SumPrice += (int)(item.Volume * item.Product.Prices.Last().price);
                tmpBlockSumm.SumTax += (int)(item.Volume * item.Product.Prices.Last().price * (item.Product.Prices.Last().taxrate / 100));
            }
            BlockSumms = tmpBlockSumm;
            CurrentBlockSummary.DataContext = null;
            CurrentBlockSummary.DataContext = BlockSumms;
        }

        /// <summary>
        /// Aktuális blok updatelése
        /// </summary>
        private void UpdateList()
        {
            CurrentBlock.DataContext = null;
            CurrentBlock.DataContext = Block;
        }

        /// <summary>
        /// Új termék bevitel befelyezve eseménykezelő
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewProductPage_OnFinishedEnty(object sender, EventArgs e)
        {
            Frame.Visibility = Visibility.Collapsed;
            BuildProductButtonList();
        }

        /// <summary>
        /// Oldal megjelenítése
        /// </summary>
        /// <param name="page">Megjelenítendő oldal</param>
        public void ShowPage(Page page)
        {
            Frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            Frame.Navigate(page);
        }

        /// <summary>
        /// Eseménykezelő a termék kezelése gombhoz
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void G1_Click(object sender, RoutedEventArgs e)
        {
            Frame.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Regex csak számokhoz
        /// </summary>
        private static readonly Regex _regex = new Regex(@"[^0-9]");
        private static bool IsTextAllowed(string text)
        {
            return _regex.IsMatch(text);
        }

        /// <summary>
        /// Eldönti hogy a kapott szöveg double-vá konvertálható e
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool IsDouble(string s)
        {
            if (s.Contains(','))
            {
                var tmp = s.Split(',');

                if (tmp.Length > 2)
                    return false;

                if (tmp.Length == 2)
                {
                    if (ContainsOnlyNumbers(tmp[0]) && ContainsOnlyNumbers(tmp[1]))
                    {
                        return true;
                    }
                    return false;
                }
                else
                {
                    return ContainsOnlyNumbers(tmp[0]);
                }
            }
            else
            {
                if(s.Length > 0)
                {
                    return ContainsOnlyNumbers(s);
                }
                else
                {
                    return true;
                }
            }

        }

        private void With_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {           
            e.Handled = !IsDouble(e.Text);
        }

        /// <summary>
        /// Eldönti hogy a kapott szöveg csak számokból áll e
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool ContainsOnlyNumbers(string s)
        {
            bool ret = true;
            foreach (var item in s)
            {
                if (!Char.IsDigit(item))
                {
                    ret = false;
                }
            }
            return ret;
        }


        private string prevText = "";

        /// <summary>
        /// Eseménykezelő 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProductNameORBarcode_KeyUp(object sender, KeyEventArgs e)
        {
            if(prevText != ProductNameORBarcode.Text)
            {
                prevText = ProductNameORBarcode.Text;

                if (!ContainsOnlyNumbers(ProductNameORBarcode.Text))
                {
                    BuildProductButtonList(ProductNameORBarcode.Text, "");
                }
                else
                {
                    BuildProductButtonList("", ProductNameORBarcode.Text);
                }

            }
        }
        /// <summary>
        /// Blok elem kijelölés változás eseménykezlő, ha kijelölés történik a törlés gomb aktívvá válik
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RemoveItem.IsEnabled = BlockListBox.SelectedItem != null;
        }
        /// <summary>
        /// Blok elem törlése eseménykezelő 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            Block.BlockItems.Remove((Models.BlockItem)BlockListBox.SelectedItem);
            UpdateList();
            RecalculateSumms();
        }
        /// <summary>
        /// Fizetve gomb eseménykezelő blokk megjelenítése és (még nem implementált) blokk adatbázisba mentése
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Fizetve_Click(object sender, RoutedEventArgs e)
        {
            var tmp = new ShowPrintBlock(Block);
            Frame.Navigate(tmp);
            Frame.Visibility = Visibility.Visible;
        }
    }
}
