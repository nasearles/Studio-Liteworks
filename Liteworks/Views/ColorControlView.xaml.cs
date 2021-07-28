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
    /// Interaction logic for ColorControlView.xaml
    /// </summary>
    public partial class ColorControlView : UserControl
    {
        public ColorControlView()
        {
            InitializeComponent();
        }
        private void btnSelect_Click(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                rectActual.Fill = rectSelected.Fill;
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
