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
using System.Windows.Shapes;
using Liteworks;

namespace Liteworks
{
    /// <summary>
    /// Interaction logic for ConnectDevicesMenu.xaml
    /// </summary>
    public partial class ConnectDevicesMenu : Window
    {
        public ConnectDevicesMenu()
        {
            InitializeComponent();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                this.Close();
            }
        }

        private void Connect_Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void AcceptClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Add1(object sender, RoutedEventArgs e)
        { 

        }
        private void Add2(object sender, RoutedEventArgs e)
        {


        }
        private void Add3(object sender, RoutedEventArgs e)
        {

        }
        private void Add4(object sender, RoutedEventArgs e)
        {

        }
        private void Add5(object sender, RoutedEventArgs e)
        {

        }
        private void Add6(object sender, RoutedEventArgs e)
        {

        }
        private void Add7(object sender, RoutedEventArgs e)
        {

        }
        private void Add8(object sender, RoutedEventArgs e)
        {

        }
    }
}
