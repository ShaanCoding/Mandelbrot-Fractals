using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandelbrot_Fractals
{
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

        //Something new I learnt for classes you can create your own custom operator definitions.
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
