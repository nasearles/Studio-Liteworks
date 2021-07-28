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
using Liteworks.ViewModels;
using Liteworks.LightingUnits;

namespace Liteworks
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int a { get; set; }
        public static int selected { get; set; }

        public static int[] unit { get; set; }

        public string Names;


        public MainWindow()
        {
            InitializeComponent();
            a = 0;
            LightingUnit bars = new LightingUnit();
            bars.Name = "Unit 1";
            Names = bars.Name;
        } 

        private void Button_Click(object sender, RoutedEventArgs e)
        { 
            
        }



        private void displayUnitTwo(object sender, RoutedEventArgs e)
        {

        }
        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void WindowSizeClick(object send, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
        }

        private void WindowMinimizeClick(object send, RoutedEventArgs e)
        {
            if (WindowState != WindowState.Minimized)
                WindowState = WindowState.Minimized;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void SideMenuClicked(object sender, RoutedEventArgs e)
        {
            if (a == 0)
            {
                DataContext = new MenuViewModel();
                a = 1;
            }
            else
            {
                DataContext = new MenuClosedViewModel();
                a = 0;
            }
            

        }


        private void RowDefinition_DragOver(object sender, DragEventArgs e)
        {

        }

        private void ExpandMenu(object sender, RoutedEventArgs e)
        {

        }


    }
}
