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

namespace WpfApp4_ImageGallery_new
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        bool isMouseCaptured;
        double mouseVerticalPosition;
        double mouseHorizontalPosition;

        public Window1()
        {
            InitializeComponent();
        }

      

        private void rect1_MouseEnter(object sender, MouseEventArgs e)
        {
            //Point mousePosition = e.GetPosition(null);
            double mousePositionX = e.GetPosition(this).X;
            double mousePositionY = e.GetPosition(this).Y;
            this.Title = mousePositionX.ToString() + " " + mousePositionY.ToString();
        }

        private void rect1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as FrameworkElement;
            mouseVerticalPosition = e.GetPosition(null).Y;
            mouseHorizontalPosition = e.GetPosition(null).X;
            isMouseCaptured = true;
            item.CaptureMouse();
        }

        private void rect1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var item = sender as FrameworkElement;
            isMouseCaptured = false;
            item.ReleaseMouseCapture();
            mouseVerticalPosition = -1;
            mouseHorizontalPosition = -1;
        }

        private void rect1_MouseMove(object sender, MouseEventArgs e)
        {
            var item = sender as FrameworkElement;
            if (isMouseCaptured)
            {

                // Calculate the current position of the object.
                double deltaV = e.GetPosition(null).Y - mouseVerticalPosition;
                double deltaH = e.GetPosition(null).X - mouseHorizontalPosition;
                double newTop = deltaV + (double)item.GetValue(Canvas.TopProperty);
                double newLeft = deltaH + (double)item.GetValue(Canvas.LeftProperty);
                if (newLeft > 0 && newTop > 0 && newLeft + rect1.Width < imageBox1.ActualWidth -1 && newTop + rect1.Height < imageBox1.ActualHeight - 1)
                {
                    // Set new position of object.
                    rect1.SetValue(Canvas.TopProperty, newTop);
                    rect1.SetValue(Canvas.LeftProperty, newLeft);

                    // Update position global variables.
                    mouseVerticalPosition = e.GetPosition(null).Y;
                    mouseHorizontalPosition = e.GetPosition(null).X;

                    if (imageBox1.Source != null)
                    {
                        var ratioX = imageBox1.Source.Width / imageBox1.ActualWidth;
                        var ratioY = imageBox1.Source.Height / imageBox1.ActualHeight;

                        Int32Rect rcFrom = new Int32Rect
                        {
                            X = (int)(Canvas.GetLeft(rect1) * ratioX),
                            Y = (int)(Canvas.GetTop(rect1) * ratioY),
                            Width = (int)(rect1.Width * ratioX),
                            Height = (int)(rect1.Height * ratioY)
                        };

                        var crop = new CroppedBitmap(imageBox1.Source as BitmapSource, rcFrom);
                        imageBox2.Source = crop;
                    }
                }
            }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (imageBox1.Source != null)
            {
                var ratioX = imageBox1.Source.Width / imageBox1.ActualWidth;
                var ratioY = imageBox1.Source.Height / imageBox1.ActualHeight;

                Int32Rect rcFrom = new Int32Rect
                {
                    X = (int)(Canvas.GetLeft(rect1) * ratioX),
                    Y = (int)(Canvas.GetTop(rect1) * ratioY),
                    Width = (int)(rect1.Width * ratioX),
                    Height = (int)(rect1.Height * ratioY)
                };

                var crop = new CroppedBitmap(imageBox1.Source as BitmapSource, rcFrom);
                imageBox2.Source =crop;
                BitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(crop));
               
                using (var fs = System.IO.File.OpenWrite("crop.jpg"))
                {
                    pngEncoder.Save(fs);
                }
            }
        }
    }
}
