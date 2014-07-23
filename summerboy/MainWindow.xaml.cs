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

namespace summerboy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WriteableBitmap wb;
        private int r, g, b;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void log(String message)
        {
            statusBox.Text += DateTime.Now.ToString("[hh:mm:ss] ") + message + "\n";
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            r = 150;
            g = 100;
            b = 100;

            var image = new BitmapImage(new Uri(""));
            wb = new WriteableBitmap(image.PixelWidth, image.PixelHeight, image.DpiX, image.DpiY, PixelFormats.Bgra32, null);
            log("initialized.");
            this.SizeToContent = SizeToContent.Width;
            imageBox.BeginInit();
            imageBox.Source = image;
            imageBox.EndInit();
            surveyBox.BeginInit();
            surveyBox.Source = wb;
            surveyBox.EndInit();
        }

        private void imageBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(this);
            p.X *= wb.PixelWidth / imageBox.ActualWidth;
            p.Y *= wb.PixelWidth / imageBox.ActualWidth;
            log("" + p.X + "," + p.Y);
            
            wb.Lock();
            unsafe
            {
                for (int i = (int)p.Y - 30; i - p.Y < 30 && i < wb.PixelHeight; i++)
                {
                    for (int j = (int)p.X - 30; j - p.X < 30 && j < wb.PixelWidth; j++)
                    {
                        int pBackBuffer = (int)wb.BackBuffer + (i * wb.BackBufferStride) + j * 4;
                        int color_data = 128 << 24 | r << 16 | g << 8 | b;
                        *((int *)pBackBuffer) |= color_data;

                    }
                }
            }

            r += 10;
            
            wb.AddDirtyRect(new Int32Rect(0, 0, wb.PixelWidth, wb.PixelHeight));
            wb.Unlock();
        }
    }
}
