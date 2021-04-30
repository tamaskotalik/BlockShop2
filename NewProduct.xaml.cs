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
    /// Interaction logic for Page1.xaml
    /// </summary>
    /// 
    public partial class NewProduct : Page
    {


        public NewProductDC DC;

        DispatcherTimer timer;
        /// <summary>
        /// Eseménykezelő page bezárásához
        /// </summary>
        public event EventHandler OnFinishedEnty;


        public NewProduct()
        {
            InitializeComponent();


            Save.IsEnabled = false;
  
            DC = new NewProductDC();
            DC.product = new Product();
            DC.price = new Price();

            MainGrid.DataContext = DC;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(30);
            timer.Tick += new EventHandler(CheckData);
            timer.IsEnabled = true;


            SetComboBox();
        }


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

        private void CheckData(object sender, EventArgs e)
        {           
            Save.IsEnabled = (DC?.product.Name?.Length > 0) && (DC?.product.Unit?.Length > 0) && (DC?.price.price > 0);
        }

        private static readonly Regex _regex = new Regex(@"[^0-9]");
        private static bool IsTextAllowed(string text)
        {
            return _regex.IsMatch(text);
        }

        private void With_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = IsTextAllowed(e.Text);
        }

        public void FinishedEntry()
        {
            OnFinishedEnty?.Invoke(this, new EventArgs());
        }

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
            DC = new NewProductDC();
            DC.product = new Product();
            DC.price = new Price();
            MainGrid.DataContext = DC;
            if (!Next.IsChecked.Value)
            {
                FinishedEntry();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            FinishedEntry();
        }

        private void TBname_KeyUp(object sender, KeyEventArgs e)
        {
            ;
        }

        private void TBname_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((Product)ComboBoxName.SelectedItem != null)
            {
                var tmpDc = new NewProductDC();
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

    public class VC_FT_To_int : IValueConverter
    {
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int ret = 0;
            var b = int.TryParse(value.ToString().Replace("Ft", "").Replace(" ", ""), out ret);
            return ret;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString() + " Ft";
        }
    }
    public class VC_Percent_To_float : IValueConverter
    {
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            float ret = 0;
            var b = float.TryParse(value.ToString().Replace("%", "").Replace(" ", ""), out ret);
            return ret;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString() + " %";
        }
    }
}
