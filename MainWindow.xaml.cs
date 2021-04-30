using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        NewProduct NewProductPage;
        public MainWindow()
        {
            InitializeComponent();

            NewProductPage = new NewProduct();
            NewProductPage.OnFinishedEnty += new EventHandler(NewProductPage_OnFinishedEnty);
            Frame.Visibility = Visibility.Collapsed;
            ShowPage(NewProductPage);
        }

        private void NewProductPage_OnFinishedEnty(object sender, EventArgs e)
        {
            Frame.Visibility = Visibility.Collapsed;
        }

        public void ShowPage(Page page)
        {
            Frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            Frame.Navigate(page);
        }

        private void G1_Click(object sender, RoutedEventArgs e)
        {
            Frame.Visibility = Visibility.Visible;
        }
    }
}
