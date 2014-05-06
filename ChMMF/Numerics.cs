using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PoohMathParser;

namespace ChMMF
{
    class Numerics
    {
        public static Tuple<double, int, int> gauss(MathExpression function, double a, double b, double accuracy)
        {
            int n = 3;

            double oldResult = 5;
            double result = oldResult + accuracy * 2;

            int number = 1;
            int iterations = 0;

            while (Math.Abs(result - oldResult) > accuracy)
            {
                oldResult = result;

                double someSum = 0;

                for (double i = a; i < b; i += (b - a) / number)
                {
                    double x1 = (i + (i + (b - a) / number)) / 2 + ((i + (b - a) / number) - i) / 2 * 0;
                    double x2 = (i + (i + (b - a) / number)) / 2 + ((i + (b - a) / number) - i) / 2 * Math.Sqrt(3 / 5);
                    double x3 = (i + (i + (b - a) / number)) / 2 + ((i + (b - a) / number) - i) / 2 * (-Math.Sqrt(3 / 5));
                    double dod1 = 8 * function.Calculate(x1) / 9;
                    double dod2 = 5 * function.Calculate(x2) / 9;
                    double dod3 = 5 * function.Calculate(x3) / 9;

                    someSum += ((i + (b - a) / number) - i) / 2 * (dod1 + dod2 + dod3);
                }

                result = someSum;

                number *= 2;

                ++iterations;
            }

            return new Tuple<double, int, int>(result, n, iterations);
        }

        private static Tuple<double, int, int> chebyshev(MathExpression function, double a, double b, double accuracy)
        {
            int n = 3;

            double oldResult = 5;
            double result = oldResult + accuracy * 2;

            int number = 1;
            int iterations = 0;

            while (Math.Abs(result - oldResult) > accuracy)
            {
                oldResult = result;

                double someSum = 0;

                for (double i = a; i < b; i += (b - a) / number)
                {
                    double x1 = (i + (i + (b - a) / number)) / 2 + ((i + (b - a) / number) - i) / 2 * 0;
                    double x2 = (i + (i + (b - a) / number)) / 2 + ((i + (b - a) / number) - i) / 2 * Math.Sqrt(3 / 5);
                    double x3 = (i + (i + (b - a) / number)) / 2 + ((i + (b - a) / number) - i) / 2 * (-Math.Sqrt(3 / 5));
                    double dod1 = 2 * function.Calculate(x1) / n;
                    double dod2 = 2 * function.Calculate(x2) / n;
                    double dod3 = 2 * function.Calculate(x3) / n;

                    someSum += ((i + (b - a) / number) - i) / 2 * (dod1 + dod2 + dod3);
                }

                result = someSum;

                number *= 2;

                ++iterations;
            }

            return new Tuple<double, int, int>(result, n, iterations);
        }

               private static Tuple<double, int, int> Simpson(MathExpression function, MathExpression a, MathExpression b,
            MathExpression c, MathExpression d, double accuracy)
        {

            double result = 0;
            double prevResult = result + accuracy * 2;
            int n = 1;
            int iterations = 0;
            double A = a.Calculate();
            double B = b.Calculate();
            double C = c.Calculate();
            double D = d.Calculate();
            double xLeft = A;
            double xRight = B;
            double yBottom = C;
            double yTop = D;
            while (Math.Abs(result - prevResult) > accuracy)
            {
                prevResult = result;
                result = 0;
                double smallResult = 1;

                double xStep = (B - A) / n;
                double yStep = (D - C) / n;

                yTop = C + yStep;
                
                for (int i = 0; i < n; ++i)
                {
                    xRight = A + xStep;

                    for (int j = 0; j < n; ++j)
                    {
                        smallResult *= (xRight - xLeft) / 36;
                        smallResult *= function.Calculate(xLeft) +
                            function.Calculate(xRight) +
                            function.Calculate(xLeft) +
                            function.Calculate(xRight) +
                            4 * (function.Calculate((xLeft + xRight) / 2) +
                            function.Calculate((xLeft + xRight) / 2) +
                            function.Calculate(xLeft, (yBottom + yTop) / 2) +
                            function.Calculate(xRight, (yBottom + yTop) / 2)) +
                            16 * function.Calculate((xLeft + xRight) / 2, (yBottom + yTop) / 2);
                        xLeft += xStep;
                        xRight += xStep;
                        result += smallResult;
                        smallResult = 1;
                    }
                    xLeft = A;
                    xRight = B;
                    yBottom += yStep;
                    yTop += yStep;
                }
                ++iterations;
                n *= 2;
                yBottom = C;
                yTop = D;
            }

            return new Tuple<double, int, int>(result, iterations, n);
        }

        private static Tuple<double, int, int> MonteCarlo(MathExpression function, MathExpression a, MathExpression b,
            MathExpression c, MathExpression d, double accuracy)
        {
            Random rnd = new Random();

            int N = 10;
            double sum = 0;
            

            double result = 0;
            double prevResult = result + accuracy * 2;
            int iterations = 0;
            int n = 1;
            Random rand = new Random();
            while (Math.Abs(result - prevResult) > accuracy)
            {
                prevResult = result;
                result = 0;
                double ab = (b.Calculate() - a.Calculate());
                double cd = (d.Calculate() - c.Calculate());
                for (int i = 0; i < n; i++)
                {
                    result += function.Calculate(a.Calculate() + ab * rand.NextDouble(), c.Calculate() + cd * rand.NextDouble());
                }
                result *= cd * ab / n;
                ++iterations;
                n *= 2;
            }

            return new Tuple<double, int, int>(result, iterations, n);
        }
    }
}
