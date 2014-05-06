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
using PoohMathParser;

namespace ChMIntegral
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public MathExpression function;
        public static MathExpression antiderivative;
        public static double a;
        public static double b;

        private string checkedRadioButtonName;

        private WindowPlot w;

        private Dictionary<string, Func<MathExpression, double, double, double, Tuple<double, int, int>>> methods =
            new Dictionary<string, Func<MathExpression, double, double, double, Tuple<double, int, int>>>
            {
                { "radioButtonRectangles", (function, a, b, accuracy) => rectangles(function, a, b, accuracy) },
                { "radioButtonLeftRectangles", (function, a, b, accuracy) => leftRectangles(function, a, b, accuracy) },
                { "radioButtonRightRectangles", (function, a, b, accuracy) => rightRectangles(function, a, b, accuracy) },
                { "radioButtonTrapeze", (function, a, b, accuracy) => trapeze(function, a, b, accuracy) },
                { "radioButtonParabola", (function, a, b, accuracy) => parabola(function, a, b, accuracy) },
                { "radioButtonGauss", (function, a, b, accuracy) => gauss(function, a, b, accuracy) },
                { "radioButtonChebyshev", (function, a, b, accuracy) => chebyshev(function, a, b, accuracy) },
            };

        private void buttonCalculate_Click(object sender, RoutedEventArgs e)
        {
            function = new MathExpression(textBoxFunction.Text);
            antiderivative = new MathExpression(textBoxAntiDerivative.Text);

            a = Double.Parse(textBoxA.Text);
            b = Double.Parse(textBoxB.Text);
            double accuracy = Double.Parse(textBoxAccuracy.Text);

            double superResult = antiderivative.Calculate(b) - antiderivative.Calculate(a);
            textBoxSuperResult.Text = superResult.ToString();

            Tuple<double, int, int> bigResult = methods[checkedRadioButtonName](function, a, b, accuracy);

            textBoxResult.Text = bigResult.Item1.ToString();
            textBoxN.Text = bigResult.Item2.ToString();
            textBoxIterations.Text = bigResult.Item3.ToString();
        }

        private string Format(double number, double accuracy)
        {
            int numOfDigitsAfterPoint = accuracy.ToString().Length - 1;
            string format = "{0:0.";
            for (int i = 0; i < numOfDigitsAfterPoint; ++i)
            {
                format += "0";
            }
            format += "}";

            string result = String.Format(format, number);
            return result;
        }

        #region Numerical methods

        private static Tuple<double, int, int> rectangles(MathExpression function, double a, double b, double accuracy)
        {
            int n = 1;
            int iterations = 0;
            double oldResult = 5;
            double result = oldResult + accuracy * 2;
            
            while (Math.Abs(result - oldResult) > accuracy) 
            {
                double h = (b - a) / n;   
                double someSum = 0;
                oldResult = result;

                for (double i = 1; i <= 2 * n - 1; i += 2)
                {
                    someSum += function.Calculate(a + i * h / 2);
                }

                result = ((b - a) / n) * someSum;

                n *= 2;
                ++iterations;
            }

            return new Tuple<double, int, int>(result, n, iterations);
        }

        private static Tuple<double, int, int> leftRectangles(MathExpression function, double a, double b, double accuracy)
        {
            int n = 1;
            int iterations = 0;
            double oldResult = 5;
            double result = oldResult + accuracy * 2;

            while (Math.Abs(result - oldResult) > accuracy)
            {
                double h = (b - a) / n;
                double someSum = 0;
                oldResult = result;

                for (double i = 0; i <= n - 1; ++i)
                {
                    someSum += function.Calculate(a + i * h);
                }

                result = ((b - a) / n) * someSum;

                n *= 2;
                ++iterations;
            }

            return new Tuple<double, int, int>(result, n, iterations);
        }

        private static Tuple<double, int, int> rightRectangles(MathExpression function, double a, double b, double accuracy)
        {
            int n = 1;
            int iterations = 0;
            double oldResult = 5;
            double result = oldResult + accuracy * 2;

            while (Math.Abs(result - oldResult) > accuracy)
            {
                double h = (b - a) / n;
                double someSum = 0;
                oldResult = result;

                for (double i = 1; i <= n; ++i)
                {
                    someSum += function.Calculate(a + i * h);
                }

                result = ((b - a) / n) * someSum;

                n *= 2;
                ++iterations;
            }

            return new Tuple<double, int, int>(result, n, iterations);
        }

        private static Tuple<double, int, int> trapeze(MathExpression function, double a, double b, double accuracy)
        {
            int n = 1;
            int iterations = 0;
            double oldResult = 5;
            double result = oldResult + accuracy * 2;

            while (Math.Abs(result - oldResult) > accuracy)
            {
                double h = (b - a) / n;
                double someSum = 0;
                oldResult = result;

                for (double i = 1; i <= n - 1; ++i)
                {
                    someSum += 2 * function.Calculate(a + i * h);
                }

                result = ((b - a) / (2 * n)) * (function.Calculate(a) + someSum + function.Calculate(b));

                n *= 2;
                ++iterations;
            }

            return new Tuple<double, int, int>(result, n, iterations);
        }

        private static Tuple<double, int, int> parabola(MathExpression function, double a, double b, double accuracy)
        {
            int n = 1;
            int iterations = 0;
            double oldResult = 5;
            double result = oldResult + accuracy * 2;

            while (Math.Abs(result - oldResult) > accuracy)
            {
                double h = (b - a) / n;
                double someSum = 0;
                oldResult = result;

                for (double i = 1; i <= n - 1; ++i)
                {
                    int multiplicator;                 
                    if (i % 2 == 0)
                    {
                        multiplicator = 2;
                    }
                    else
                    {
                        multiplicator = 4;
                    }
                    someSum += multiplicator * function.Calculate(a + i * h);
                }

                result = ((b - a) / (3 * n)) * (function.Calculate(a) + someSum + function.Calculate(b));

                n *= 2;
                ++iterations;
            }

            return new Tuple<double, int, int>(result, n, iterations);
        }

        private static Tuple<double, int, int> gauss(MathExpression function, double a, double b, double accuracy)
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

        #endregion

        private void radioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton chckd = sender as RadioButton;
            checkedRadioButtonName = chckd.Name;
        }

        private void buttonDrawPlot_Click(object sender, RoutedEventArgs e)
        {
            if (w != null)
            {
                w.Close();
            }
            w = new WindowPlot();
            w.Show();
        }
    }
}
