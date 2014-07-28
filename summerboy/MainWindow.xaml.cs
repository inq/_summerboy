using Microsoft.Win32;
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
        private String ipt;

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
            Imagination.ImageLoader ldr = new Imagination.ImageLoader();
            log("" + ldr.get724());
            ldr.set724(235);
            log("" + ldr.get724());

            r = 150;
            g = 100;
            b = 100;

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Image Files|*.jpg";
            openFileDialog1.Title = "Choose the base";

            if (openFileDialog1.ShowDialog() != true)
                return;

            ipt = openFileDialog1.FileName;
            var image = new BitmapImage(new Uri(openFileDialog1.FileName));
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

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.R:
                    OpenFileDialog openFileDialog1 = new OpenFileDialog();
                    openFileDialog1.Multiselect = true;
                    openFileDialog1.Filter = "Image Files|*.jpg";
                    openFileDialog1.Title = "Choose the targets";

                    if (openFileDialog1.ShowDialog() != true)
                        return;

                    var hello = new Imagination.Processor(ipt, openFileDialog1.FileNames);
                    
                    
                    break;
                case Key.Space:
                    log("SPACE");
                    break;
            }
        }
    }
}
