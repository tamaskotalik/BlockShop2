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
using System.Windows.Threading;
using Models;

namespace BlockShop2
{
    /// <summary>
    /// Új termék felvétele / módosítása oldal osztálya
    /// </summary>
    /// 
    public partial class NewProduct : Page
    {

        /// <summary>
        /// Adat contexus a databindigekhez
        /// </summary>
        public ProductDataContext DC;
        /// <summary>
        /// Időzítő az adatok ellenőrzéséhez mentés gomb elérhetővé tételéhez
        /// </summary>
        DispatcherTimer timer;

        /// <summary>
        /// Eseménykezelő page bezárásához
        /// </summary>
        public event EventHandler OnFinishedEnty;

        /// <summary>
        /// Construktor és inicializálások
        /// </summary>
        public NewProduct()
        {
            InitializeComponent();


            Save.IsEnabled = false;
  
            DC = new ProductDataContext();
            DC.product = new Product();
            DC.price = new Price();

            MainGrid.DataContext = DC;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(30);
            timer.Tick += new EventHandler(CheckData);
            timer.IsEnabled = true;


            SetComboBox();
        }

        /// <summary>
        /// Termék név kombó box beállítása
        /// </summary>
        private void SetComboBox()
        {
            Controller c = new Controller();
            var items = c.GetProducts();
            ComboBoxName.Items.Clear();

            foreach (var item in items)
            {
                ComboBoxName.Items.Add(item);
            }
            ComboBoxName.IsTextSearchCaseSensitive = true;
            ComboBoxName.IsTextSearchEnabled = true; 
        }
        /// <summary>
        /// Mentés gomb elérhetővé tétele
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckData(object sender, EventArgs e)
        {           
            Save.IsEnabled = (DC?.product.Name?.Length > 0) && (DC?.product.Unit?.Length > 0) && (DC?.price.price > 0);
        }
        /// <summary>
        /// Csak számok regex minta
        /// </summary>
        private static readonly Regex _regex = new Regex(@"[^0-9]");

        /// <summary>
        /// Kapott szöveg alapján döntés hogy engedélyezett e (_regex minta felhasználásával)
        /// </summary>
        /// <param name="text">Ellenőrizendő szöveg</param>
        /// <returns></returns>
        private static bool IsTextAllowed(string text)
        {
            return _regex.IsMatch(text);
        }


        /// <summary>
        /// Eseménykezelő a szöveg bevitel előtti ellenőrzéséhez
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void With_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = IsTextAllowed(e.Text);
        }

        /// <summary>
        /// Eseménykezelő hívása ablak bezárásához
        /// </summary>
        public void FinishedEntry()
        {
            OnFinishedEnty?.Invoke(this, new EventArgs());
        }


        /// <summary>
        /// Mentés gomb eseménykezelője
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Controller c = new Controller();            
            
            if(ComboBoxName.SelectedItem == null)
            {
                DC.product.ProductId = 0;
                var tmp = c.AddOrUpdate(DC.product, DC.price);
                ComboBoxName.Items.Add(tmp);
            }
            else
            {
                var tmp = c.AddOrUpdate(DC.product, DC.price);
            }
            DC = new ProductDataContext();
            DC.product = new Product();
            DC.price = new Price();
            MainGrid.DataContext = DC;
            if (!Next.IsChecked.Value)
            {
                FinishedEntry();
            }
        }
        /// <summary>
        /// Mégse gomb eseménykezelője
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            FinishedEntry();
        }

        private void TBname_KeyUp(object sender, KeyEventArgs e)
        {
            ;
        }
        /// <summary>
        /// Név combobox selectionchanged eseménykezelője
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TBname_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((Product)ComboBoxName.SelectedItem != null)
            {
                var tmpDc = new ProductDataContext();
                tmpDc.product = ((Product)ComboBoxName.SelectedItem);

                var tmp = ((Product)ComboBoxName.SelectedItem).Prices;

                if (tmp.Count > 0)
                {
                    tmpDc.price = tmp.Last();
                }
                else
                {
                    tmpDc.price = new Price();
                }

                DC = tmpDc;
                MainGrid.DataContext = DC;
            }
 
        }
    }
}
