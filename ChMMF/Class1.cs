using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PoohMathParser;
using System.Windows.Forms;

namespace ChMMF
{
    public class Calulator
    {
        double[,] A;
        double[,] M;
        double[] L;
        double[] U0;
        MathExpression f;
        MathExpression u0;
        MathExpression q;
        double T;
        double TN;
        int N;
        string u0text;
        string fText;
        double dt;
        double[] c;
        public Calulator()
        {
        }
        public Calulator(MathExpression m1, MathExpression m2, MathExpression m3, double d1, double d2, int i, string s1, string s2)
        {
            u0text = s1;
            fText = s2;
            N = i;
            T = d1;
            TN = d2;
            q = m1;
            f = m3;
            u0 = m2;
            A = new double[N, N];
            M = new double[N, N];
            L = new double[N];
            U0 = new double[N];
        }
        public void calcA()
        {
            double h = 1.0 / N;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (j == i - 1)
                    {
                        A[i, j] = -1 / h;
                    }
                    else if (j == i + 1 || (j == N - 1 && i == N - 1))
                    {
                        A[i, j] = 1 / h;

                    }
                    else if (i == j)
                    {
                        A[i, j] = 2 / h;
                    }
                    else
                    {
                        A[i, j] = 0;
                    }
                }
            }
        }
        public void calcL(double tj1)
        {
            double h = 1.0 / N;
            string fTextNew = fText.Replace("t", tj1.ToString());
            // MessageBox.Show(fTextNew);
            for (int i = 1; i < N; i++)
            {
                double xi_1 = h * (i - 1);
                double xi1 = h * (i + 1);
                double xi = h * (i);
                if (i != N)
                {

                    MathExpression m1 = new MathExpression("(" + fTextNew + ")*(x-" + xi_1.ToString() + ")");
                    MathExpression m2 = new MathExpression("(" + fTextNew + ")*(" + xi1.ToString() + "-x)");
                    L[i] = (Numerics.gauss(m1, xi_1, xi, 0.0001).Item1 + Numerics.gauss(m2, xi, xi1, 0.0001).Item1) / h;
                }
                else
                {
                    MathExpression m1 = new MathExpression("(" + fTextNew + ")*(x-" + xi_1.ToString() + ")");
                    L[i] = Numerics.gauss(m1, xi_1, xi, 0.0001).Item1 / h - q.Calculate(tj1);
                }
            }
        }
        public void calcU0()
        {
            //MathExpression tempU0 = "("+u0text+")*()"
            double h = 1.0 / N;
            for (int i = 1; i < N + 1; i++)
            {
                double xi_1 = h * (i - 1);//x i-1
                MathExpression tempU01 = new MathExpression("(" + u0text + ")*(x-" + xi_1.ToString() + ")");//first part
                if (i != N)
                {
                    double xi1 = h * (i + 1);//x i+1
                    MathExpression tempU02 = new MathExpression("(" + u0text + ")*(" + xi1.ToString() + "-x)");//second part
                    U0[i - 1] = Numerics.gauss(tempU01, xi_1, h * i, 0.0001).Item1 / h + Numerics.gauss(tempU02, xi1, h * i, 0.0001).Item1 / h;
                }
                else
                {
                    U0[i - 1] = Numerics.gauss(tempU01, xi_1, h * i, 0.0001).Item1 / h;
                }

            }
        }
        public void calcM()
        {
            double h = 1.0 / N;
            for (int i = 1; i < N + 1; i++)
            {
                double xi1 = h * (i + 1);
                double xi = h * (i);
                double xi_1 = h * (i - 1);
                for (int j = 1; j < N + 1; j++)
                {
                    if (j == i - 1)
                    {

                        MathExpression mTemp = new MathExpression("(x-" + xi_1.ToString() + ")*(" + xi.ToString() + "-x)");
                        M[i - 1, j - 1] = Numerics.gauss(mTemp, xi_1, xi, 0.0001).Item1 / (h * h);
                        //MessageBox.Show("(x-" + xi_1.ToString() + ")*(" + xi.ToString() + "-x)/" + (h * h).ToString());
                    }
                    else if (j == i + 1)
                    {

                        MathExpression mTemp = new MathExpression("(x-" + xi.ToString() + ")*(" + xi1.ToString() + "-x)");
                        M[i - 1, j - 1] = Numerics.gauss(mTemp, xi, xi1, 0.0001).Item1 / (h * h);
                        //MessageBox.Show(i.ToString()+j.ToString());
                    }
                    else if (i == j)
                    {
                        if (i != N)
                        {
                            MathExpression mTemp2 = new MathExpression("(" + xi1.ToString() + "-x)*(" + xi1.ToString() + "-x)");
                            MathExpression mTemp1 = new MathExpression("(x-" + xi_1.ToString() + ")*(x-" + xi_1.ToString() + ")");
                            M[i - 1, j - 1] = (Numerics.gauss(mTemp1, xi_1, xi, 0.0001).Item1 + Numerics.gauss(mTemp2, xi, xi1, 0.0001).Item1) / (h * h);
                        }
                        else
                        {
                            MathExpression mTemp1 = new MathExpression("(x-" + xi_1.ToString() + ")*(x-" + xi_1.ToString() + ")");
                            M[i - 1, j - 1] = Numerics.gauss(mTemp1, xi, xi_1, 0.0001).Item1 / (h * h);
                        }
                    }
                    else
                    {
                        M[i - 1, j - 1] = 0;
                    }
                }

            }

        }
        public double[] mult(double[,] matr, double[] vect)
        {
            double[] rez = new double[N];
            for (int i = 0; i < N; i++)
            {
                rez[i] = 0;
                for (int j = 0; j < N; j++)
                {
                    rez[i] += matr[i, j] * vect[j];
                }
            }
            return rez;
        }
        public double[] minus(double[] row, double[] vect)
        {
            double[] rez = new double[N];
            for (int i = 0; i < N; i++)
            {
                rez[i] = row[i] - vect[i];
            }
            return rez;
        }
        public double[,] plusMatr(double[,] a, double[,] b)
        {
            double[,] rez = new double[N, N];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    rez[i, j] = a[i, j] + b[i, j];
                }
            }
            return rez;
        }
        double[,] multByNumber(double[,] a, double d)
        {
            double[,] rez = new double[N, N];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    rez[i, j] = a[i, j] * d;
                }
            }
            return rez;
        }
        double[] addRow(double[] a, double[] b)
        {
            double[] rez = new double[N];
            for (int i = 0; i < N; i++)
            {
                rez[i] = a[i] + b[i];
            }
            return rez;
        }
        double[] multRow(double[] a, double b)
        {
            double[] rez = new double[N];
            for (int i = 0; i < N; i++)
            {
                rez[i] = a[i] * b;
            }
            return rez;
        }
        public void initialize()
        {
            calcM();
            calcA();
            calcU0();
            dt = TN / N;

        }
        public double[] calculate(int idx)
        {
            initialize();
            if (idx == 0)
            {
                c = solve(M, U0, N);
            }
            else
            {
                double t = dt * idx;
                calcL(t);
                double[] right = minus(L, mult(A, c));
                double[,] left = plusMatr(M, multByNumber(A, dt));
                double[] cDot = solve(left, right, N);
               // MessageBox.Show("l" + L[0].ToString() + L[1].ToString() + L[2].ToString() + L[3].ToString() + L[4].ToString());
                c = addRow(c, multRow(cDot, dt / 2));
                MessageBox.Show("cdot" + multRow(cDot, dt / 2)[0].ToString() + ' ' + multRow(cDot, dt / 2)[1].ToString() + ' ' + (dt / 25).ToString() + ' ' + cDot[3].ToString() + ' ' + cDot[4].ToString());
                MessageBox.Show("c" + c[0].ToString() + ' ' + c[1].ToString() + ' ' + c[2].ToString() + ' ' + c[3].ToString() + ' ' + c[4].ToString());
            }
            
            return c;
        }

        public double[] solve(double[,] matr, double[] d, int n)
        {
            n--; // since we start from x0 (not x1)
            matr[0, 1] /= matr[0, 0];//c[0] = c[0]/b[0];
            d[0] = d[0] / matr[0, 0];

            for (int i = 1; i < n; i++)
            {
                matr[i, i + 1] /= matr[i, i] - matr[i, i - 1] * matr[i - 1, i];//c[i] /= b[i] - a[i] * c[i - 1];
                d[i] = (d[i] - matr[i, i - 1] * d[i - 1]) / (matr[i, i] - matr[i, i - 1] * matr[i - 1, i]);
            }

            d[n] = (d[n] - matr[n, n - 1] * d[n - 1]) / (matr[n, n] - matr[n, n - 1] * matr[n - 1, n]);//d[n] = (d[n] - a[n] * d[n - 1]) / (b[n] - a[n] * c[n - 1]);

            for (int i = n; i-- > 0; )
            {
                d[i] -= matr[i, i + 1] * d[i + 1];
            }
            return d;
        }
    }
}
