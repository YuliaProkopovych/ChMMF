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
using System.Windows.Shapes;
using PoohMathParser;

namespace ChMIntegral
{
    /// <summary>
    /// Interaction logic for WindowPlot.xaml
    /// </summary>
    public partial class WindowPlot : Window
    {
        public WindowPlot()
        {
            InitializeComponent();
            this.Title = "Графік функції " + MainWindow.antiderivative.ToString() + "=0";
            canvasPlot.Children.Clear();
            drawPlot(MainWindow.antiderivative);
        }

        public double scale = 10;

        private void drawAxis()
        {
            
            Point center = new Point(canvasPlot.Width / 2, canvasPlot.Height / 2);
            Line xAxis = new Line();
            xAxis.Stroke = System.Windows.Media.Brushes.DarkGray;
            xAxis.X1 = 0;
            xAxis.Y1 = canvasPlot.Height / 2;
            xAxis.X2 = xAxis.X1 + canvasPlot.Width;
            xAxis.Y2 = xAxis.Y1;
            canvasPlot.Children.Add(xAxis);
            Polyline xArrow = new Polyline();
            xArrow.Stroke = System.Windows.Media.Brushes.DarkGray;
            PointCollection xArrowPoints = new PointCollection();
            xArrowPoints.Add(new Point(xAxis.X2 - 8, xAxis.Y2 - 5));
            xArrowPoints.Add(new Point(xAxis.X2, xAxis.Y2));
            xArrowPoints.Add(new Point(xAxis.X2 - 8, xAxis.Y2 + 5));
            xArrow.Points = xArrowPoints;
            canvasPlot.Children.Add(xArrow);
            Label x = new Label();
            x.Content = "x";
            Canvas.SetTop(x, canvasPlot.Height / 2);
            Canvas.SetLeft(x, canvasPlot.Width - 12);
            x.Foreground = System.Windows.Media.Brushes.DarkGray;
            canvasPlot.Children.Add(x);
            Line yAxis = new Line();
            yAxis.Stroke = System.Windows.Media.Brushes.DarkGray;
            yAxis.X1 = canvasPlot.Width / 2;
            yAxis.Y1 = 0;
            yAxis.X2 = yAxis.X1;
            yAxis.Y2 = yAxis.Y1 + +canvasPlot.Height;
            canvasPlot.Children.Add(yAxis);
            Polyline yArrow = new Polyline();
            yArrow.Stroke = System.Windows.Media.Brushes.DarkGray;
            PointCollection yArrowPoints = new PointCollection();
            yArrowPoints.Add(new Point(yAxis.X1 - 5, yAxis.Y1 + 8));
            yArrowPoints.Add(new Point(yAxis.X1, yAxis.Y1));
            yArrowPoints.Add(new Point(yAxis.X1 + 5, yAxis.Y1 + 8));
            yArrow.Points = yArrowPoints;
            canvasPlot.Children.Add(yArrow);
            Label fx = new Label();
            fx.Content = "f(x)";
            Canvas.SetTop(fx, -3);
            Canvas.SetLeft(fx, canvasPlot.Width / 2 + 5);
            fx.Foreground = System.Windows.Media.Brushes.DarkGray;
            canvasPlot.Children.Add(fx);
        }
        public void drawRange()
        {
            Point center = new Point(canvasPlot.Width / 2, canvasPlot.Height / 2);

            Rectangle r = new Rectangle();
            Canvas.SetLeft(r, center.X + MainWindow.a * scale);
            Canvas.SetTop(r, 0);
            r.Width = (MainWindow.b - MainWindow.a) * scale;
            r.Height = canvasPlot.Height;
            r.Fill = System.Windows.Media.Brushes.Lavender;

            canvasPlot.Children.Add(r);
        }
        private List<Tuple<double, double>> getRangesWithoutBreakPoints(MathExpression expr)
        {
            List<Tuple<double, double>> rangesWithoutBreakPoints = new List<Tuple<double, double>>();
            double left = (-canvasPlot.Width / (2 * scale));
            for (double x = (-canvasPlot.Width / (2 * scale)); x < (canvasPlot.Width / (2 * scale)); x += 0.1 / scale)
            {
                double right = left - 1;
                while (!double.IsNaN(expr.Calculate(x))
                    && (x < canvasPlot.Width / (2 * scale))
                    && (expr.Calculate(x) < canvasPlot.Height / (scale * 0.1))
                    && (expr.Calculate(x) > -canvasPlot.Height / scale * 2))
                {
                    right = x;
                    x += 0.1 / scale;
                }
                if (right > left)
                {
                    Tuple<double, double> range = new Tuple<double, double>(left, right);
                    rangesWithoutBreakPoints.Add(range);
                    left = right + 0.1 / scale;
                }
                else
                {
                    left = x + 0.1 / scale;
                }
            }
            return rangesWithoutBreakPoints;
        }
        private void drawPlot(MathExpression expr)
        {
            drawRange();
            drawAxis();
            
            Point center = new Point(canvasPlot.Width / 2, canvasPlot.Height / 2);

            List<Tuple<double, double>> rangesWithoutBreakPoints = getRangesWithoutBreakPoints(expr);
            foreach (Tuple<double, double> t in rangesWithoutBreakPoints)
            {
                Polyline plot = new Polyline();
                plot.Stroke = System.Windows.Media.Brushes.Black;
                PointCollection points = new PointCollection();
                for (double x = t.Item1; x < t.Item2; x += 0.5 / scale)
                {
                    Point forrest = new Point();
                    forrest.X = center.X + x * scale;
                    forrest.Y = canvasPlot.Height - (center.Y + expr.Calculate(x) * scale);
                    points.Add(forrest);
                }
                plot.Points = points;
                canvasPlot.Children.Add(plot);
            }
        }

        private void canvasLabel_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePosition = e.GetPosition(canvasLabel);
            Point center = new Point(canvasLabel.Width / 2, canvasLabel.Height / 2);
            if (canvasLabel.Width - mousePosition.X < textBlockMousePos.ActualWidth)
            {
                textBlockMousePos.SetValue(Canvas.LeftProperty, mousePosition.X - textBlockMousePos.ActualWidth);
            }
            else
            {
                textBlockMousePos.SetValue(Canvas.LeftProperty, mousePosition.X + 13);
            }
            if (canvasLabel.Height - mousePosition.Y < textBlockMousePos.ActualHeight)
            {
                textBlockMousePos.SetValue(Canvas.TopProperty, mousePosition.Y - textBlockMousePos.ActualHeight);
            }
            else
            {
                textBlockMousePos.SetValue(Canvas.TopProperty, mousePosition.Y + 14);
            }
            double xCoord = (-canvasLabel.Width / 2 + mousePosition.X) / scale;
            double yCoord = (canvasLabel.Height / 2 - mousePosition.Y) / scale;
            textBlockMousePos.Content = String.Format("({0:0.##};{1:0.##})", xCoord, yCoord);
        }
        private void canvasLabel_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                scale *= 1.5;
            }
            if (e.Delta < 0)
            {
                scale /= 1.5;
            }
            canvasPlot.Children.Clear();
            drawPlot(MainWindow.antiderivative);
        }
    }
}