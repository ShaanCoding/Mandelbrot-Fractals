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

        double centerX, centerY;
        double zoomScale;

        int numColors = 85;
        int height = 600;
        int width = 800;
        int max;

        int maxColors = 1000;

        private ColorArray colorArray;

        public Mandrelbrot(double zoom, double centerX, double centerY, int max)
        {
            this.max = max;
            if(max < maxColors)
            {
                colorArray = new ColorArray(max, max);
            }
            else
            {
                colorArray = new ColorArray(maxColors, maxColors);
            }

            this.zoomScale = zoom;
            this.centerX = centerX;
            this.centerY = centerY;
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
            return returnBMP;
        }
    }

    class ComplexPoint
    {
        public double R;
        public double Im;

        public ComplexPoint(double R, double Im)
        {
            this.R = R;
            this.Im = Im;
        }

        public static double Abs(ComplexPoint value)
        {
            double c = Math.Abs(value.R);
            double d = Math.Abs(value.Im);

            if (c > d)
            {
                double r = d / c;
                return c * Math.Sqrt(1.0 + r * r);
            }
            else if (d == 0.0)
            {
                return c;  // c is either 0.0 or NaN
            }
            else
            {
                double r = c / d;
                return d * Math.Sqrt(1.0 + r * r);
            }
        }

        public static ComplexPoint operator +(ComplexPoint x, ComplexPoint y)
        {
            return new ComplexPoint(x.R + y.R, x.Im + y.Im);
        }

        public static ComplexPoint operator *(ComplexPoint x, ComplexPoint y)
        {
            return new ComplexPoint(x.R * y.R - x.Im * y.Im, x.R * y.Im + x.Im * y.R);
        }
    }
}
