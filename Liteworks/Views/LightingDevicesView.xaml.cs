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

namespace Liteworks.Views
{
    /// <summary>
    /// Interaction logic for LightingDevicesView.xaml
    /// </summary>
    public partial class LightingDevicesView : UserControl
    {
        public LightingDevicesView()
        {
            InitializeComponent();
        }
        private void displayUnit(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).BorderBrush != Brushes.Green)
            {
                ((Button)sender).BorderBrush = Brushes.Green;
                ((Button)sender).IsEnabled = true;
            }
            else
            {
                ((Button)sender).BorderBrush = Brushes.MidnightBlue;
                ((Button)sender).IsEnabled = false;
            }


        }
    }
}
