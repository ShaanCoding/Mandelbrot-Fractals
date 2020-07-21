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
        int max = 85;
        bool zoomBool = true;
        int width = 800;
        int height = 600;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Mandelbrot_Fractals_ContentRendered(object sender, EventArgs e)
        {
            mandrelbrot = new Mandrelbrot(width, height, zoom, centerX, centerY, max);
            MandrelbrotImage.Source = Convert(mandrelbrot.mandrelbrotFractalBMP());
        }

        private void MandrelbrotImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(zoomBool)
            {
                System.Windows.Point p = e.GetPosition(this);

                //Width & height
                double minX = centerX - zoom / 2.0;
                double minY = centerY - zoom / 2.0;

                centerX = minX + (double)p.X / width * zoom;
                centerY = minY + (double)p.Y / height * zoom;

                //ZoomScale
                zoom -= 3 * zoom / 10;

                UpdateLabels();

                mandrelbrot = new Mandrelbrot(width, height, zoom, centerX, centerY, max);
                MandrelbrotImage.Source = Convert(mandrelbrot.mandrelbrotFractalBMP());
            }
        }

        private void GeneratePatternButton_Click(object sender, RoutedEventArgs e)
        {
            Int32.TryParse(IterationTextBox.Text, out int iterations);
            max = iterations;

            Int32.TryParse(ZoomScaleTextBox.Text, out int zoomScale);
            zoom = zoomScale;

            Int32.TryParse(CenterXTextBox.Text, out int XCenter);
            centerX = XCenter;

            Int32.TryParse(CenterYTextBox.Text, out int YCenter);
            centerY = YCenter;

            mandrelbrot = new Mandrelbrot(width, height, zoom, centerX, centerY, max);
            MandrelbrotImage.Source = Convert(mandrelbrot.mandrelbrotFractalBMP());
        }

        private void ZoomCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            zoomBool = (bool)ZoomCheckBox.IsChecked;
        }

        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
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
        }

        private void UpdateLabels()
        {
            ZoomScaleTextBox.Text = zoom.ToString();
            IterationTextBox.Text = max.ToString();
            CenterXTextBox.Text = centerX.ToString();
            CenterYTextBox.Text = centerY.ToString();
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
    }
}
