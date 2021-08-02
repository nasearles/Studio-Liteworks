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
using System.IO.Ports;
using Liteworks.Views;

namespace Liteworks
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int a { get; set; }
        public static int c { get; set; }
        public static int selected { get; set; }

        public static int[] unit { get; set; }

        public string Names;

        MenuViewModel hold;

        SerialPort serialPort;

        //string[] ports;


        public MainWindow()
        {
            InitializeComponent();
            a = 0;
            c = 0;
            LightingUnit bars = new LightingUnit();
            bars.Name = "Unit 1";
            Names = bars.Name;
            serialPort = new SerialPort();
        } 

       

        private void Button_Click(object sender, RoutedEventArgs e)
        { 
            
        }

        private void displayUnit(object sender, RoutedEventArgs e)
        {
            ((Button)sender).Content = Names;
            if (((Button)sender).BorderBrush != Brushes.Green)
                ((Button)sender).BorderBrush = Brushes.Green;

            else
                ((Button)sender).BorderBrush = Brushes.MidnightBlue;
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
                if (c == 0)
                {
                    hold = new MenuViewModel();
                    c = 1;
                }
                DataContext = hold;
                a = 1;
            }
            else
            {
                serialPort = MenuView.GetPort();
                DataContext = new MenuClosedViewModel();
                a = 0;
            }
            

        }

        private void SideMenuUnclick(object sender, RoutedEventArgs e)
        {
            DataContext = new MenuClosedViewModel();
        }

        private void RowDefinition_DragOver(object sender, DragEventArgs e)
        {

        }

        private void ExpandMenu(object sender, RoutedEventArgs e)
        {

        }

        private void btnSelect_Click(object sender, MouseButtonEventArgs e)
        {
            if(!serialPort.IsOpen)
                serialPort = MenuView.GetPort();

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                rectActual.Fill = rectSelected.Fill;
                SolidColorBrush cb = rectActual.Fill as SolidColorBrush;
                if (cb != null)//&& ((string)serialConnectButton.Content == "Disconnect")
                {
                    Color c = cb.Color;
                    byte[] pix = new byte[3];
                    pix[0] = c.R;
                    pix[1] = c.G;
                    pix[2] = c.B;
                    serialPort.Write(pix, 0, 3);

                }
            }
            
        }

        //Change color cube to correlate with the brightness slider
        private void slider_Color_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (rectColor_Background != null)
            {
                double imgOpacity = slider_Color.Value;
                imgCubeWhite.Opacity = imgOpacity;
            }
        }

        //Get color from cube on mouse hover
        private void imgCubeWhite_MouseMove(object sender, MouseEventArgs e)
        {
            select_Color(sender, e);
        }


        //Gets color from cursor position relative to the bitmap of the hovered image
        private void select_Color(object sender, MouseEventArgs e)
        {
            try
            {
                BitmapSource visual_BitmapSource = get_BitmapSource_of_Element(imgCubeWhite);
                CroppedBitmap cb = new CroppedBitmap(visual_BitmapSource, new Int32Rect((int)Mouse.GetPosition(imgCubeWhite).X, (int)Mouse.GetPosition(imgCubeWhite).Y, 1, 1));
                byte[] pixels = new byte[4];

                try
                {
                    cb.CopyPixels(pixels, 4, 0);
                }
                catch (Exception)
                {
                    //error
                }
                rectSelected.Fill = new SolidColorBrush(Color.FromRgb(pixels[2], pixels[1], pixels[0]));



            }
            catch (Exception)
            {
                //not much we can do i guess
            }
        }

       
        public BitmapSource get_BitmapSource_of_Element(FrameworkElement element)
        {
            // <Check>
            if (element == null) { return null; }
            double dpi = 96;
            Double width = element.ActualWidth;
            Double height = element.ActualHeight;

            // init

            RenderTargetBitmap bitmap_of_Element = null;
            if (bitmap_of_Element == null)
            {
                try
                {
                    //create an empty Bitmap of element
                    bitmap_of_Element = new RenderTargetBitmap((int)width, (int)height, dpi, dpi, PixelFormats.Default);

                    // render area into bitmap
                    DrawingVisual visual_area = new DrawingVisual();
                    using (DrawingContext dc = visual_area.RenderOpen())
                    {
                        VisualBrush visual_brush = new VisualBrush(element);
                        dc.DrawRectangle(visual_brush, null, new Rect(new Point(), new Size(width, height)));
                    }


                    //render
                    bitmap_of_Element.Render(visual_area);

                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return bitmap_of_Element;
        }





        //byte byte_Shade = Convert.ToByte(slider_Color.Value *255)
        //rectColor_Background.Fill = new SolidColorBrush(Color.FromRgb(byte_shade, byte_shade, byte_shade));
        //gradStopCenter.Color = Color.FromRgb(byte_shade, byte_shade, byte_shade); 
    }
}
