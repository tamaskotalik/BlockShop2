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
using CefSharp;

namespace BlockShop2
{
    /// <summary>
    /// Interaction logic for ShowPrintBlock.xaml
    /// </summary>
    public partial class ShowPrintBlock : Page
    {        
        public ShowPrintBlock()
        {
            InitializeComponent();            
        }

        public ShowPrintBlock(Models.Block actblock)
        {
            InitializeComponent();
            double sum = 0;
            double summtax = 0;
            string text = LoadHeader();

            if(actblock != null)
            {
                foreach (var item in actblock.BlockItems)
                {
                    text += "<div>" + item.Product.Name + " " + item.Product.LastPrice.price.ToString() + " " + "</div>";
                }
            }
            text += LoadFooter();
            Browser.Width = 400;
            Browser.NavigateToString(text);
        }
/*
        public static void Send(Key key)
        {
            if (Keyboard.PrimaryDevice != null)
            {
                if (Keyboard.PrimaryDevice.ActiveSource != null)
                {
                    var e1 = new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Down) {
                        RoutedEvent = Keyboard.KeyDownEvent
                    };
                    e1.
                    InputManager.Current.ProcessInput(e1);
                }
            }
        }*/

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
        /*    var e2 = new RoutedEventArgs()
            Browser.RaiseEvent()
            Send(Key.LeftCtrl);
            Send(Key.P);*/
        }
    }
}
