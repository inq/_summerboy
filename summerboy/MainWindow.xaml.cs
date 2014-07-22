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
            log("initialized.");
            this.SizeToContent = SizeToContent.Width;
            imageBox.BeginInit();
            imageBox.Source = new BitmapImage(new Uri(""));
            imageBox.EndInit();
        }
    }
}
