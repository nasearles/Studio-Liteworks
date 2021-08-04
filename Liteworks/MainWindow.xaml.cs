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

        public bool[] unit { get; set; }

        public string Names;

        public byte[] pix;

        MenuViewModel hold;

        SerialPort serialPort;

        //string[] ports;


        public MainWindow()
        {
            InitializeComponent();
            a = 0;
            c = 0;
            LightingUnit bars = new LightingUnit();
            unit = new bool[8];
            for(int i = 0; i < 8; i++)
            {
                unit[i] = false;
            }
            bars.Name = "Unit 1";
            Names = bars.Name;
            serialPort = new SerialPort();
            pix = new byte[5];
        } 

       
        private void HoverAnimation(object sender, MouseEventArgs e)
        {
            ((Image)sender).Opacity = 0;
            
        }

        private void Unhover(object sender, MouseEventArgs e)
        {
            ((Image)sender).Opacity = 1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        { 
            
        }

        private void displayUnit(object sender, RoutedEventArgs e)
        {
            //((Button)sender).Content = Names;
            if (((Button)sender).BorderBrush != Brushes.Green)
            {
                ((Button)sender).BorderBrush = Brushes.Green;
                unit[int.Parse(((Button)sender).Name.Substring(1, 1)) - 1] = true;
            }
            else
            {
                ((Button)sender).BorderBrush = Brushes.MidnightBlue;
                unit[int.Parse(((Button)sender).Name.Substring(1,1)) - 1] = false;
            }
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


        private void btnSelect_Click(object sender, MouseButtonEventArgs e)
        {
            if(!serialPort.IsOpen)
                if(MenuView.GetPort() != null)
                    serialPort = MenuView.GetPort();

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                rectActual.Fill = rectSelected.Fill;
                SolidColorBrush cb = rectActual.Fill as SolidColorBrush;
                if (cb != null && serialPort.IsOpen)
                {
                    Color c = cb.Color;
                    pix[0] = 0;
                    pix[1] = c.R;
                    pix[2] = c.G;
                    pix[3] = c.B;
                    pix[4] = ConvertBoolArrayToByte(unit);
                    serialPort.Write(pix, 0, 5);

                }
            }
            
        }

        //Change color cube to correlate with the brightness slider
        private void slider_Color_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (rectColor_Background != null)
            {
                double imgOpacity = slider_Color.Value / 255;
                imgCubeWhite.Opacity = imgOpacity;
            }

            if (serialPort == null)
            {
                if (MenuView.GetPort() != null)
                {
                    serialPort = MenuView.GetPort();
                }
            }

            if (serialPort != null)
            {
                if (serialPort.IsOpen)
                {
                    pix[0] = 8;
                    pix[1] = (byte)slider_Color.Value;
                    pix[2] = 0;
                    pix[3] = 0;
                    pix[4] = ConvertBoolArrayToByte(unit);
                    serialPort.Write(pix, 0, 5);
                }

            }
        }

        private void slider_Timing_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (serialPort == null)
            {
                if (MenuView.GetPort() != null)
                {
                    serialPort = MenuView.GetPort();
                }
            }

            if (serialPort != null)
            {
                if (serialPort.IsOpen)
                {
                    pix[0] = 9;
                    pix[1] = (byte)slider_Timing.Value;
                    pix[2] = 0;
                    pix[3] = 0;
                    pix[4] = ConvertBoolArrayToByte(unit);
                    serialPort.Write(pix, 0, 5);
                }

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

        private void ledRainbow(object sender, MouseButtonEventArgs e)
        {
            if (!serialPort.IsOpen)
                if (MenuView.GetPort() != null)
                    serialPort = MenuView.GetPort();

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                rectActual.Fill = rectSelected.Fill;
                SolidColorBrush cb = rectActual.Fill as SolidColorBrush;
                if (cb != null && serialPort.IsOpen)
                {
                    Color c = cb.Color;
                    pix[0] = 1;
                    pix[1] = 0;
                    pix[2] = 0;
                    pix[3] = 0;
                    pix[4] = ConvertBoolArrayToByte(unit);
                    serialPort.Write(pix, 0, 5);

                }
            }
        }

        private void ledBounce(object sender, MouseButtonEventArgs e)
        {
            if (!serialPort.IsOpen)
                if (MenuView.GetPort() != null)
                    serialPort = MenuView.GetPort();

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                rectActual.Fill = rectSelected.Fill;
                SolidColorBrush cb = rectActual.Fill as SolidColorBrush;
                if (cb != null && serialPort.IsOpen)
                {
                    Color c = cb.Color;
                    pix[0] = 6;
                    pix[1] = 0;
                    pix[2] = 0;
                    pix[3] = 0;
                    pix[4] = ConvertBoolArrayToByte(unit);
                    serialPort.Write(pix, 0, 5);

                }
            }
        }
        private void Sinelon(object sender, MouseButtonEventArgs e)
        {
            if (!serialPort.IsOpen)
                if (MenuView.GetPort() != null)
                    serialPort = MenuView.GetPort();

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                rectActual.Fill = rectSelected.Fill;
                SolidColorBrush cb = rectActual.Fill as SolidColorBrush;
                if (cb != null && serialPort.IsOpen)
                {
                    Color c = cb.Color;
                    pix[0] = 4;
                    pix[1] = 0;
                    pix[2] = 0;
                    pix[3] = 0;
                    pix[4] = ConvertBoolArrayToByte(unit);
                    serialPort.Write(pix, 0, 5);

                }
            }
        }
        private void Confetti(object sender, MouseButtonEventArgs e)
        {
            if (!serialPort.IsOpen)
                if (MenuView.GetPort() != null)
                    serialPort = MenuView.GetPort();

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                rectActual.Fill = rectSelected.Fill;
                SolidColorBrush cb = rectActual.Fill as SolidColorBrush;
                if (cb != null && serialPort.IsOpen)
                {
                    Color c = cb.Color;
                    pix[0] = 3;
                    pix[1] = 0;
                    pix[2] = 0;
                    pix[3] = 0;
                    pix[4] = ConvertBoolArrayToByte(unit);
                    serialPort.Write(pix, 0, 5);

                }
            }
        }
        private void ledSparkles(object sender, MouseButtonEventArgs e)
        {
            if (!serialPort.IsOpen)
                if (MenuView.GetPort() != null)
                    serialPort = MenuView.GetPort();

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                rectActual.Fill = rectSelected.Fill;
                SolidColorBrush cb = rectActual.Fill as SolidColorBrush;
                if (cb != null && serialPort.IsOpen)
                {
                    Color c = cb.Color;
                    pix[0] = 2;
                    pix[1] = 0;
                    pix[2] = 0;
                    pix[3] = 0;
                    pix[4] = ConvertBoolArrayToByte(unit);
                    serialPort.Write(pix, 0, 5);

                }
            }
        }
        private void ledJuggle(object sender, MouseButtonEventArgs e)
        {
            if (!serialPort.IsOpen)
                if (MenuView.GetPort() != null)
                    serialPort = MenuView.GetPort();

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                rectActual.Fill = rectSelected.Fill;
                SolidColorBrush cb = rectActual.Fill as SolidColorBrush;
                if (cb != null && serialPort.IsOpen)
                {
                    Color c = cb.Color;
                    pix[0] = 5;
                    pix[1] = 0;
                    pix[2] = 0;
                    pix[3] = 0;
                    pix[4] = ConvertBoolArrayToByte(unit);
                    serialPort.Write(pix, 0, 5);

                }
            }
        }

        private static byte ConvertBoolArrayToByte(bool[] source)
        {
            byte result = 0;
            // This assumes the array never contains more than 8 elements!
            int index = 8 - source.Length;

            // Loop through the array
            foreach (bool b in source)
            {
                // if the element is 'true' set the bit at that position
                if (b)
                    result |= (byte)(1 << (7 - index));

                index++;
            }

            return result;
        }




        //byte byte_Shade = Convert.ToByte(slider_Color.Value *255)
        //rectColor_Background.Fill = new SolidColorBrush(Color.FromRgb(byte_shade, byte_shade, byte_shade));
        //gradStopCenter.Color = Color.FromRgb(byte_shade, byte_shade, byte_shade); 
    }
}
