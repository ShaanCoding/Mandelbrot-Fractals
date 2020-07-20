using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

namespace Mandelbrot_Fractals
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Mandrelbrot mandrelbrot;
        double zoom = 5;
        double centerX = 0, centerY = 0;
        int max = 100;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Mandelbrot_Fractals_ContentRendered(object sender, EventArgs e)
        {
            mandrelbrot = new Mandrelbrot(zoom, centerX, centerY, max);
            MandrelbrotImage.Source = Convert(mandrelbrot.mandrelbrotFractalBMP());
        }

        public static BitmapImage Convert(Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

        private void MandrelbrotImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Point p = e.GetPosition(this);
            double minX = centerX - zoom / 2.0, minY = centerY - zoom / 2.0;

            //Width & height
            centerX = minX + (double)p.X / 800 * zoom;
            centerY = minY + (double)p.Y / 600 * zoom;

            double zoomMinus = 3 * zoom / 10;
            zoom -= zoomMinus;
            max += 50;

            mandrelbrot = new Mandrelbrot(zoom, centerX, centerY, max);
            MandrelbrotImage.Source = Convert(mandrelbrot.mandrelbrotFractalBMP());

            /*
            using(CommonSaveFileDialog saveFileDialog = new CommonSaveFileDialog())
            {
                saveFileDialog.AlwaysAppendDefaultExtension = true;
                saveFileDialog.Filters.Add(new CommonFileDialogFilter("PNG Files", "*.png"));
                saveFileDialog.DefaultExtension = "png";

                if(saveFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    mandrelbrot.mandrelbrotFractalBMP().Save(saveFileDialog.FileName);
                }
            }
            */
        }
    }
}
