using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChM1
{


    public static class Integrals
    {
        public enum Formula { Left, Center, Trapez, Simphson, Gauss };
        public static double Function(double x)
        {
            return Math.Exp(-x * x) * 2 / Math.Sqrt(Math.PI);
        }

        public static double SmallIntegral(Formula formula, double x1, double x2)
        {
            switch (formula)
            {
                case Formula.Left:
                    return (x2 - x1) * Function(x1);
                case Formula.Center:
                    return (x2 - x1) * Function((x2 + x1) / 2);
                case Formula.Trapez:
                    return ((x2 - x1) / 2) * (Function(x1) + Function(x2));
                case Formula.Simphson:
                    return ((x2 - x1) / 6) * (Function(x1) + 4 * Function(x1 + (x2 - x1) / 2) + Function(x2));
                case Formula.Gauss:
                    return ((x2 - x1) / 2) * (Function(x1 + ((x2 - x1) / 2) * (1 - 1 / Math.Sqrt(3)))) +
                                              Function(x1 + ((x2 - x1) / 2) * (1 + 1 / Math.Sqrt(3)));
            }
            return 0;
        }

        public static double Integral(Formula formula, double a, double x, double eps, out int k)
        {
            double sumk = 0, sum2k = 0;
            k = 2;
            double h = (x - a) / k;
            double x1 = a, x2 = x1 + h;

            for (int i = 0; i < k; i++)
            {
                sumk += SmallIntegral(formula, x1, x2);
                x1 = x2;
                x2 += h;
            }

            bool flag = true;
            while(flag)
            {
                k *= 2;
                h = (x - a) / k;
                x1 = a;
                x2 = x1 + h;
                sum2k = 0;
                for (int i = 0; i < k; i++)
                {
                    sum2k += SmallIntegral(formula, x1, x2);
                    x1 = x2;
                    x2 += h;
                }
                if (Math.Abs(sumk - sum2k) < eps)
                    flag = false;
                sumk = sum2k;
            }
            return sumk;
        }
    }

    //public static 
}