using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ChM1
{

    class Program
    {
        static double qdif(double x, int n)
        {
            return (-1)*(x*x) / (n + 1);
        }

        static double[] TabulationDif(double eps, double[] nodes)
        {
            double an;
            double[] values = new double[nodes.Length];
            int n;
            for (int i = 0; i < nodes.Length; i++)
            {
                n = 0;
                an = (2 / Math.Sqrt(Math.PI));
                values[i] = 0;

                while (Math.Abs(an) >= eps)
                {
                    values[i] += an;
                    an *= qdif(nodes[i], n);// вычисляем a(n+1)-ый
                    n++;
                }         
            }

            return values;
        }

        static double[] Error(double[] ipl, double[] values)
        {
            double[] error;
            if (ipl.Length <= values.Length)
                error = new double[ipl.Length];
            else
                error = new double[values.Length];

            for (int i = 0; i < error.Length; i++)
            {
                error[i] = Math.Abs(ipl[i] - values[i]);
            }

            return error;
        }

        static void MassivOut(double[] mass, StreamWriter sw)
        {
            for (int i = 0; i < mass.Length; i++)
            {
                sw.WriteLine(mass[i].ToString());
            }
            sw.WriteLine();
        }

        static double l(int k, double x, double[] nodes)
        {
            double l = 0;
            for (int j = 0; j < nodes.Length; j++)
            {
                if (j != k)
                {
                    double proizv = 1;
                    for (int i = 0; i < nodes.Length; i++)
                    {
                        if (i == k)
                            continue;
                        if (i == j)
                            continue;
                        proizv *= x - nodes[i];
                    }
                    
                    double proizv2 = 1;
                    for (int i = 0; i < nodes.Length; i++)
                    {
                        if(i!=k)
                            proizv2 *= nodes[k] - nodes[i];
                    }
                    if (proizv2 != 0)
                        proizv = proizv / proizv2;
                    else 
                        continue;
                    l += proizv;
                }
            }
            return l;
        }

        static double[] IPLdif(double[] nodes, double[] values, double[] args)
        {
            double[] output = new double[args.Length];
            double[] input = args;
            double[] nodesInput = nodes;
            double[] nodesOutput = values;
            int k = input.Length;
            int nodesLength = nodes.Length;
            int n = nodesLength;
            double x, p, p1, p2, p3, s, s2;
            for(int t=0;t<k;t++)
            {
                x = input[t];
                s = 0;
                p = 1;
                for (int i = 0; i < n; i++)
                {
                    p = nodesOutput[i];
                    s2 = 0;
                    for (int j = 0; j < n; j++)
                    {
                        if (j != i)
                        {
                            p1 = 1;
                            for (int m = 0; m < n; m++)
                            {
                                if (m != i && m != j)
                                {
                                    p1 = p1 * (x - nodesInput[m]);
                                }
                            }
                            p2 = 1;
                            for (int c = 0; c < n; c++)
                                if (c != i)
                                    p2 *= nodesInput[i] - nodesInput[c];
                            s2 += p1 / p2;
                        }
                    }
                    p *= s2;
                    s += p;
                }
                output[t] = s;
            }
            return output;
            
        }

        static double[] IPLdif2(double[] nodes, double[] values, double[] args)
        {
            double[] output = new double[args.Length];

            for (int m = 0; m < args.Length; m++)
            {
                output[m] = 0;
                for (int k = 0; k < nodes.Length; k++)
                {
                    output[m] += values[k]*l(k, args[m], nodes);
                }
            }

            return output;
        }

        static double[] IPL(double[] nodes, double[] values, double[] args)
        {
            double[] output = new double[args.Length];
            for (int k = 0; k < args.Length; k++)
            {
                output[k] = 0;
                for (int i = 0; i < nodes.Length; i++)
                {
                    double proizv = values[i];
                    for (int j = 0; j < nodes.Length; j++)
                    {
                        if(j!=i && nodes[i] != nodes[j])
                            proizv *= (args[k] - nodes[j]) / (nodes[i] - nodes[j]);
                    }
                    output[k] += proizv;
                }
            }
            return output;
        }

        static double[] NodesH(double a, double b, double h)
        {
            int n;
            double[] nodes;
            if ((double)(int)((b - a) / h) != (b - a) / h)
                n = (int)((b - a) / h + 2);
            else
                n = (int)((b - a) / h + 1);

            nodes = new double[n];
            for (int i = 0; i < n - 1; i++)
            {
                nodes[i] = a + i * h;
            }
            nodes[n - 1] = b;
            return nodes;
        }
        
        static double[] NodesN(double a, double b, int n)
        {
            double h = (b - a) / n;
            double[] nodes = new double[n+1];
            for (int i = 0; i < n; i++)
            {
                nodes[i] = a + i * h;
            }
            nodes[n] = b;
            return nodes;

        }

        static double q(double x, int n)
        {
            return ((-1) * (x * x) * (2 * n + 1)) / ((n + 1) * (2 * n + 3));
        }

        static double[] Tabulation(double eps, double[] nodes)
        {
            double sum, an;
            double[] values = new double[nodes.Length];
            int n;
            for (int i = 0; i < nodes.Length; i++)
            {
                n = 0;
                an = (2/Math.Sqrt(Math.PI)) * nodes[i];
                sum = an;

                while (Math.Abs(an*q(nodes[i], n)) >= eps)
                {
                    an *= q(nodes[i], n);// вычисляем a(n+1)-ый
                    sum += an;
                    n++;
                }

                values[i] = sum;
            }

            return values;
        }

        static void Task1(StreamWriter sw)
        {
            double a = 0, b = 2, h = 0.2, eps = 0.000001;
            double[] nodes = NodesH(a, b, h);

            sw.WriteLine("Протабулируем с помощью разложения в ряд Тейлора");
            MassivOut(Tabulation(eps,nodes), sw);
        }

        static void Task2Dif(StreamWriter sw)
        {
            double a = 0, b = 2, eps = 0.000001;

            //Stack<double> errors = new Stack<double>();
            sw.WriteLine();
            sw.WriteLine("Производная");
            sw.WriteLine("Погрешности. h = 1, n = 2");
            double[] nodes = NodesN(a, b, 2);
            double[] args = NodesN(a, b, 4);
            double[] values = Tabulation(eps, nodes);
            double[] ipl = IPLdif(nodes, values, args);
            double[] error = Error(ipl, TabulationDif(eps, args));
            MassivOut(args, sw);
            sw.WriteLine("Погрешности. h = 1, n = 2");
            sw.WriteLine(error.Max().ToString());
            sw.WriteLine();
            MassivOut(error, sw);  
            //errors.Push(error.Max());
            sw.WriteLine();

            sw.WriteLine("h = 0.5, n = 4");
            nodes = NodesN(a, b, 4);
            args = NodesN(a, b, 8);
            values = Tabulation(eps, nodes);
            ipl = IPLdif(nodes, values, args);
            error = Error(ipl, TabulationDif(eps, args));
            MassivOut(args, sw);
            sw.WriteLine("h = 0.5, n = 4");
            sw.WriteLine(error.Max().ToString());
            sw.WriteLine();
            MassivOut(error, sw);
            //errors.Push(error.Max());
            //sw.WriteLine(error.Max().ToString());
            sw.WriteLine();

            sw.WriteLine("h = 0.4, n = 5");
            nodes = NodesN(a, b, 5);
            args = NodesN(a, b, 10);
            values = Tabulation(eps, nodes);
            ipl = IPLdif(nodes, values, args);
            error = Error(ipl, TabulationDif(eps, args));
            MassivOut(args, sw);
            sw.WriteLine("h = 0.4, n = 5");
            sw.WriteLine(error.Max().ToString());
            sw.WriteLine();
            MassivOut(error, sw);
            //errors.Push(error.Max());
            //sw.WriteLine(error.Max().ToString());
            sw.WriteLine();

            sw.WriteLine("h = 0.25, n = 8");
            nodes = NodesN(a, b, 8);
            args = NodesN(a, b, 16);
            values = Tabulation(eps, nodes);
            ipl = IPLdif(nodes, values, args);
            error = Error(ipl, TabulationDif(eps, args));
            MassivOut(args, sw);
            sw.WriteLine("h = 0.25, n = 8");
            sw.WriteLine(error.Max().ToString());
            sw.WriteLine();
            MassivOut(error, sw);
            //errors.Push(error.Max());
            //sw.WriteLine(error.Max().ToString());
            sw.WriteLine();

            sw.WriteLine("Погрешности. h = 0.2, n = 10");
            nodes = NodesN(a, b, 10);
            args = NodesN(a, b, 20);
            values = Tabulation(eps, nodes);
            ipl = IPLdif(nodes, values, args);
            error = Error(ipl, TabulationDif(eps, args));
            MassivOut(args, sw);
            sw.WriteLine("Погрешности. h = 0.2, n = 10");
            sw.WriteLine(error.Max().ToString());
            sw.WriteLine();
            MassivOut(error, sw);
            //errors.Push(error.Max());
            //sw.WriteLine(error.Max().ToString());
            sw.WriteLine();

            sw.WriteLine("Погрешности. h = 0.2, n = 16");
            nodes = NodesN(a, b, 16);
            args = NodesN(a, b, 32);
            values = Tabulation(eps, nodes);
            ipl = IPLdif(nodes, values, args);
            error = Error(ipl, TabulationDif(eps, args));
            MassivOut(args, sw);
            sw.WriteLine("Погрешности. h = 0.2, n = 16");
            sw.WriteLine(error.Max().ToString());
            sw.WriteLine();
            MassivOut(error, sw);
            // errors.Push(error.Max());
            //sw.WriteLine(error.Max().ToString());
            sw.WriteLine();

            sw.WriteLine("Погрешности. h = 0.1, n = 20");
            nodes = NodesN(a, b, 20);
            args = NodesN(a, b, 40);
            values = Tabulation(eps, nodes);
            ipl = IPLdif(nodes, values, args);
            error = Error(ipl, TabulationDif(eps, args));
            MassivOut(args, sw);
            sw.WriteLine("Погрешности. h = 0.1, n = 20");
            sw.WriteLine(error.Max().ToString());
            sw.WriteLine();
            MassivOut(error, sw);
            //errors.Push(error.Max());
            //sw.WriteLine(error.Max().ToString());
            sw.WriteLine();

            sw.WriteLine("Погрешности. h = 0.8, n = 25");
            nodes = NodesN(a, b, 25);
            args = NodesN(a, b, 50);
            values = Tabulation(eps, nodes);
            ipl = IPLdif(nodes, values, args);
            error = Error(ipl, TabulationDif(eps, args));
            MassivOut(args, sw);
            sw.WriteLine("Погрешности. h = 0.8, n = 25");
            sw.WriteLine(error.Max().ToString());
            sw.WriteLine();
            MassivOut(error, sw);
            //errors.Push(error.Max());
            //sw.WriteLine(error.Max().ToString());
            sw.WriteLine("gsdfgsdg");
            //while (errors.Count != 0)
            //{
            //    sw.WriteLine(errors.Pop().ToString());
            //}
        }

        static void Task2(StreamWriter sw)
        {
            double a = 0, b = 2, eps = 0.000001;
            
            sw.WriteLine("h = 0.5, n = 4");
            double[] nodes = NodesN(a, b, 4);
            double[] args = NodesN(a, b, 8);
            double[] values = Tabulation(eps, nodes);
            double[] ipl = IPL(nodes, values, args);
            double[] error = Error(ipl, Tabulation(eps, args));
            MassivOut(args, sw);
            MassivOut(error, sw);
            sw.WriteLine("Максимальная погрешность " + error.Max().ToString());
            sw.WriteLine();

            sw.WriteLine("Погрешности. h= 0.4  n = 5");
            nodes = NodesN(a, b, 5);
            args = NodesN(a, b, 10);
            values = Tabulation(eps, nodes);
            ipl = IPL(nodes, values, args);
            error = Error(ipl, Tabulation(eps, args));
            MassivOut(args, sw);
            MassivOut(error, sw);
            sw.WriteLine("Максимальная погрешность " + error.Max().ToString());
            sw.WriteLine();

            sw.WriteLine("Погрешности. h= 0.25  n = 8");
            nodes = NodesN(a, b, 8);
            args = NodesN(a, b, 16);
            values = Tabulation(eps, nodes);
            ipl = IPL(nodes, values, args);
            error = Error(ipl, Tabulation(eps, args));
            MassivOut(args, sw);
            MassivOut(error, sw);
            sw.WriteLine("Максимальная погрешность " + error.Max().ToString());
            sw.WriteLine();

            sw.WriteLine("Погрешности. h = 0.2, n = 10");
            nodes = NodesN(a, b, 10);
            args = NodesN(a, b, 20);
            values = Tabulation(eps, nodes);
            ipl = IPL(nodes, values, args);
            error = Error(ipl, Tabulation(eps,args));
            MassivOut(args, sw);
            MassivOut(error, sw);
            sw.WriteLine("Максимальная погрешность " + error.Max().ToString());
            sw.WriteLine();

            sw.WriteLine("Погрешности. h = 0.125,  n = 16");
            nodes = NodesN(a, b, 16);
            args = NodesN(a, b, 32);
            values = Tabulation(eps, nodes);
            ipl = IPL(nodes, values, args);
            error = Error(ipl, Tabulation(eps, args));
            MassivOut(args, sw);
            MassivOut(error, sw);
            sw.WriteLine("Максимальная погрешность " + error.Max().ToString());
            sw.WriteLine();

            sw.WriteLine("Погрешности. h = 0.1  n = 20");
            nodes = NodesN(a, b, 20);
            args = NodesN(a, b, 40);
            values = Tabulation(eps, nodes);
            ipl = IPL(nodes, values, args);
            error = Error(ipl, Tabulation(eps, args));
            MassivOut(args, sw);
            MassivOut(error, sw);
            sw.WriteLine("Максимальная погрешность " + error.Max().ToString());
            sw.WriteLine();

            sw.WriteLine("Погрешности. h = 0.8, n = 25");
            nodes = NodesN(a, b, 25);
            args = NodesN(a, b, 50);
            values = Tabulation(eps, nodes);
            ipl = IPL(nodes, values, args);
            error = Error(ipl, Tabulation(eps, args));
            MassivOut(args, sw);
            MassivOut(error, sw);
            sw.WriteLine("Максимальная погрешность " + error.Max().ToString());
            sw.WriteLine();

           }

        static double[] NodesC(double a, double b, int n)
        {
            double[] nodes = new double[n];
            for (int i = 1; (i-1) < nodes.Length; i++)
            {
                nodes[i-1] = ((b - a) / 2) * Math.Cos((2 * i - 1) * Math.PI / (2 * n)) + (b + a) / 2;
            }          
            return nodes;
        }

        static void TaskChebysh(StreamWriter sw)
        {
            double a = 0, b = 2, eps = 0.000001;
            sw.WriteLine("Погрешности.  n = 8");
            double[] nodes = NodesC(a, b, 8);
            double[] args = NodesN(a, b, 16);
            double[] values = Tabulation(eps, nodes);
            double[] ipl = IPL(nodes, values, args);
            double[] error = Error(ipl, Tabulation(eps, args));
            MassivOut(args, sw);
            sw.WriteLine(error.Max().ToString());
            sw.WriteLine();
            MassivOut(error, sw);
            sw.WriteLine();

            sw.WriteLine("Погрешности.  n = 10");
            nodes = NodesC(a, b, 10);
            args = NodesN(a, b, 20);
            values = Tabulation(eps, nodes);
            ipl = IPL(nodes, values, args);
            error = Error(ipl, Tabulation(eps, args));
            MassivOut(args, sw);
            sw.WriteLine(error.Max().ToString());
            sw.WriteLine();
            MassivOut(error, sw);
            sw.WriteLine();

            sw.WriteLine("Погрешности.  n = 40");
            nodes = NodesC(a, b, 40);
            args = NodesN(a, b, 80);
            values = Tabulation(eps, nodes);
            ipl = IPL(nodes, values, args);
            error = Error(ipl, Tabulation(eps, args));
            MassivOut(args, sw);
            sw.WriteLine(error.Max().ToString());
            sw.WriteLine();
            MassivOut(error, sw);
            sw.WriteLine();

            sw.WriteLine("Погрешности.  n = 100");
            nodes = NodesC(a, b, 100);
            args = NodesN(a, b, 200);
            values = Tabulation(eps, nodes);
            ipl = IPL(nodes, values, args);
            error = Error(ipl, Tabulation(eps, args));
            MassivOut(args, sw);
            sw.WriteLine(error.Max().ToString());
            sw.WriteLine();
            MassivOut(error, sw);
            sw.WriteLine();
            
        }

        static void TaskIntegrals(StreamWriter sw)
        {
            double a = 0, b = 2;
            int k;

            sw.WriteLine("Left");
            sw.WriteLine(Integrals.Integral(Integrals.Formula.Left, a, b, 0.000001, out k));
            sw.WriteLine(k);
            sw.WriteLine();

            sw.WriteLine("Center, eps = 0.000001");
            sw.WriteLine(Integrals.Integral(Integrals.Formula.Center, a, b, 0.000001, out k));
            sw.WriteLine(k);
            sw.WriteLine();

            sw.WriteLine("Center, eps = 0.0000001");
            sw.WriteLine(Integrals.Integral(Integrals.Formula.Center, a, b, 0.0000001, out k));
            sw.WriteLine(k);
            sw.WriteLine();

            double[] d = new double[1];
            d[0] = 2;
            MassivOut(Tabulation(0.000000001, d), sw);
        }

        static void Main(string[] args)
        {
            StreamWriter sw = new StreamWriter("out.txt");
            StreamWriter swDif = new StreamWriter("diff.txt");
            StreamWriter sw2 = new StreamWriter("chebysh.txt");
            StreamWriter swInt = new StreamWriter("integrals.txt");
                        
            Task1(sw);

            Task2(sw);

            Task2Dif(swDif);

            TaskChebysh(sw2);

            TaskIntegrals(swInt);

            sw.Close();
            sw2.Close();
            swDif.Close();
            swInt.Close();
        }
    }
}
