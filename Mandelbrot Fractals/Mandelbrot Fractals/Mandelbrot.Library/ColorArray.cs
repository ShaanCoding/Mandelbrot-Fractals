using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandelbrot_Fractals
{
    public class ColorArray
    {
        private int numColor;
        private Color[] colorArray;

        public ColorArray(int numColor)
        {
            this.numColor = numColor;
            colorArray = new Color[numColor];

            for (int i = 0; i < numColor; i++)
            {
                double colourIndex = ((double)i) / numColor;
                double hue = Math.Pow(colourIndex, 0.25);
                colorArray[i] = colorFromHSLA(hue, 0.9, 0.6);
            }
        }

        public Color GetColor(int index)
        {
            if(index < colorArray.Length)
            {
                return colorArray[index];
            }
            else
            {
                return Color.Transparent;
            }
        }
        private static Color colorFromHSLA(double H, double S, double L)
        {
            double v;
            double r, g, b;

            r = L;   // Set RGB all equal to L, defaulting to grey.
            g = L;
            b = L;

            v = (L <= 0.5) ? (L * (1.0 + S)) : (L + S - L * S);

            if (v > 0)
            {
                double m;
                double sv;
                int sextant;
                double fract, vsf, mid1, mid2;

                m = L + L - v;
                sv = (v - m) / v;
                H *= 6.0;
                sextant = (int)H;
                fract = H - sextant;
                vsf = v * sv * fract;
                mid1 = m + vsf;
                mid2 = v - vsf;

                switch (sextant)
                {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;

                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;

                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;

                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                        break;

                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;

                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }
            }

            // Create Color object from RGB values.
            Color color = Color.FromArgb((int)(r * 255), (int)(g * 255), (int)(b * 255));
            return color;
        }
    }
}
