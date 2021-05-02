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
    /// Interaction logic for DataBtn.xaml
    /// </summary>
    public partial class DataBtn : UserControl
    {
        private ProductDataContext productButtonDataContex;

        public event EventHandler OnClickSelected;

        public DataBtn()
        {
            InitializeComponent();
        }

        public DataBtn(ProductDataContext productButtonDataContex)
        {
            InitializeComponent();
            this.productButtonDataContex = productButtonDataContex;
            CustomProductButton.DataContext = productButtonDataContex;
        }

        private void OnClickSelect(object sender, RoutedEventArgs e)
        {
            OnClickSelected?.Invoke(this, e);
        }

        private void ClickSelected(object sender, RoutedEventArgs e)
        {
            OnClickSelected?.Invoke(productButtonDataContex, e);
        }
    }
}
