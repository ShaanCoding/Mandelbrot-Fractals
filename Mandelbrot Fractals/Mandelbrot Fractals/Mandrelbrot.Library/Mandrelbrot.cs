using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandelbrot_Fractals
{
    class Mandrelbrot
    {
        //Screen space 1920 * 1080
        //Image space -2, 1
        //To convert X screen space to image space X'
        //X' = (X / 1080) * (1 - (-2)) + (-2)

        /*
            Screen Space => [Smin, Smax]
            Image Space => [IMin, IMax]

            X' = ((X - SMin) / (SMax - SMin)) * (IMax - IMin) + IMin
         */

        private double centerX;
        private double centerY;
        private double zoomScale;
        private int height;
        private int width;
        private int max;

        private const int maxColors = 1000;
        private ColorArray colorArray;

        public Bitmap mandrelBrotBMP;

        public Mandrelbrot(int width, int height, double zoom, double centerX, double centerY, int max)
        {
            this.max = max;
            this.zoomScale = zoom;
            this.centerX = centerX;
            this.centerY = centerY;
            this.width = width;
            this.height = height;

            colorArray = (max < maxColors) ? new ColorArray(max) : new ColorArray(maxColors);
        }

        public Bitmap mandrelbrotFractalBMP()
        {
            Bitmap returnBMP = new Bitmap(width, height);

            LockBitmap lockBitmap = new LockBitmap(returnBMP);
            lockBitmap.LockBits();

            double minX = centerX - zoomScale / 2;
            double minY = centerY - zoomScale / 2;

            Parallel.For(0, width, (x) =>
            {
                Parallel.For(0, height, (y) =>
                {

                    double a = minX + (double)x / width * zoomScale;
                    double b = minY + (double)y / height * zoomScale;

                    ComplexPoint C = new ComplexPoint(a, b);
                    ComplexPoint Z = new ComplexPoint(0, 0);

                    int iterations = 0;
                    for (iterations = 0; iterations < max; iterations++)
                    {
                        Z = Z * Z;
                        Z = Z + C;

                        double magnitude = ComplexPoint.Abs(Z);

                        if (magnitude > 2.0)
                        {
                            break;
                        }
                    }

                    if (iterations < max)
                    {
                        lockBitmap.SetPixel(x, y, colorArray.GetColor(iterations));
                    }
                    else
                    {
                        lockBitmap.SetPixel(x, y, System.Drawing.Color.Black);
                    }
                });
            });

            lockBitmap.UnlockBits();

            mandrelBrotBMP = returnBMP;
            return returnBMP;
        }
    }
}
