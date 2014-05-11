using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using GraphLib;

namespace ChMMF
{
    public partial class Form1 : Form
    {
        private int NumGraphs;
        private String CurExample = "NORMAL";
        private Calulator c;
        int N;
        double TN;
        public Form1()
        {
            InitializeComponent();
            display.Smoothing = System.Drawing.Drawing2D.SmoothingMode.None;
        }
        private String RenderYLabel(DataSource s, float value)
        {
            return String.Format("{0:0.0}", value);
        }
        private String RenderXLabel(DataSource s, int idx)
        {
            if (s.AutoScaleX)
            {
                if (idx % 2 == 0)
                {
                    int Value = (int)(s.Samples[idx].x);
                    return "" + Value;
                }
                return "";
            }
            else
            {
                int Value = (int)(s.Samples[idx].x / 200);
                String Label = "" + Value + "\"";
                return Label;
            }
        }
        protected void CalcMyFunction(DataSource src, int idx, double[] c)
        {
            double koef = 10.0 / (1*(c.Max() - c.Min()));
            {
                for (int i = 0; i < src.Length; i++)
                {
                    src.Samples[i].x = i *(10);
                    src.Samples[i].y = (float)(c[i] * koef);
                }
            }


               
            
        }
        protected void CalcDataGraphs()
        {

            this.SuspendLayout();

            display.DataSources.Clear();
            display.SetDisplayRangeX(0, (int)TN*10);

            for (int j = 0; j < NumGraphs; j++)
            {
                display.DataSources.Add(new DataSource());
                display.DataSources[j].Name = "Graph " + (j + 1);
                display.DataSources[j].OnRenderXAxisLabel += RenderXLabel;

                switch (CurExample)
                {
                    case "NORMAL":
                        this.Text = "Normal Graph";
                        display.DataSources[j].Length = (int)TN+1;
                        display.PanelLayout = PlotterGraphPaneEx.LayoutMode.NORMAL;
                            display.DataSources[j].AutoScaleY = false;
                        display.DataSources[j].AutoScaleX = false;
                        double[] cc = c.calculate(j);
                        display.DataSources[j].SetDisplayRangeY(-20,20);
                        display.DataSources[j].SetGridDistanceY((float)2);
                        CalcMyFunction(display.DataSources[j], j,cc);
 display.DataSources[j].OnRenderYAxisLabel = RenderYLabel; 
                        break;

                    case "NORMAL_AUTO":
                        this.Text = "Normal Graph Autoscaled";
                        display.DataSources[j].Length = 5800;
                        display.PanelLayout = PlotterGraphPaneEx.LayoutMode.NORMAL;
                        display.DataSources[j].AutoScaleY = true;
                        display.DataSources[j].SetDisplayRangeY(-300, 300);
                        display.DataSources[j].SetGridDistanceY(100);
                        display.DataSources[j].OnRenderYAxisLabel = RenderYLabel;
                        //CalcSinusFunction_0(display.DataSources[j], j);
                        break;

                    case "STACKED":
                        this.Text = "Stacked Graph";
                        display.PanelLayout = PlotterGraphPaneEx.LayoutMode.STACKED;
                        display.DataSources[j].Length = 5800;
                        display.DataSources[j].AutoScaleY = false;
                        display.DataSources[j].SetDisplayRangeY(-250, 250);
                        display.DataSources[j].SetGridDistanceY(100);
                        //CalcSinusFunction_1(display.DataSources[j], j);
                        break;

                    case "VERTICAL_ALIGNED":
                        this.Text = "Vertical aligned Graph";
                        display.PanelLayout = PlotterGraphPaneEx.LayoutMode.VERTICAL_ARRANGED;
                        display.DataSources[j].Length = 5800;
                        display.DataSources[j].AutoScaleY = false;
                        display.DataSources[j].SetDisplayRangeY(-300, 300);
                        display.DataSources[j].SetGridDistanceY(100);
                        //CalcSinusFunction_2(display.DataSources[j], j);
                        break;

                    case "VERTICAL_ALIGNED_AUTO":
                        this.Text = "Vertical aligned Graph autoscaled";
                        display.PanelLayout = PlotterGraphPaneEx.LayoutMode.VERTICAL_ARRANGED;
                        display.DataSources[j].Length = 5800;
                        display.DataSources[j].AutoScaleY = true;
                        display.DataSources[j].SetDisplayRangeY(-300, 300);
                        display.DataSources[j].SetGridDistanceY(100);
                        //CalcSinusFunction_2(display.DataSources[j], j);
                        break;

                    case "TILED_VERTICAL":
                        this.Text = "Tiled Graphs (vertical prefered)";
                        display.PanelLayout = PlotterGraphPaneEx.LayoutMode.TILES_VER;
                        display.DataSources[j].Length = 5800;
                        display.DataSources[j].AutoScaleY = false;
                        display.DataSources[j].SetDisplayRangeY(-300, 600);
                        display.DataSources[j].SetGridDistanceY(100);
                        //CalcSinusFunction_2(display.DataSources[j], j);
                        break;

                    case "TILED_VERTICAL_AUTO":
                        this.Text = "Tiled Graphs (vertical prefered) autoscaled";
                        display.PanelLayout = PlotterGraphPaneEx.LayoutMode.TILES_VER;
                        display.DataSources[j].Length = 5800;
                        display.DataSources[j].AutoScaleY = true;
                        display.DataSources[j].SetDisplayRangeY(-300, 600);
                        display.DataSources[j].SetGridDistanceY(100);
                        //CalcSinusFunction_2(display.DataSources[j], j);
                        break;

                    case "TILED_HORIZONTAL":
                        this.Text = "Tiled Graphs (horizontal prefered)";
                        display.PanelLayout = PlotterGraphPaneEx.LayoutMode.TILES_HOR;
                        display.DataSources[j].Length = 5800;
                        display.DataSources[j].AutoScaleY = false;
                        display.DataSources[j].SetDisplayRangeY(-300, 600);
                        display.DataSources[j].SetGridDistanceY(100);
                        //CalcSinusFunction_2(display.DataSources[j], j);
                        break;

                    case "TILED_HORIZONTAL_AUTO":
                        this.Text = "Tiled Graphs (horizontal prefered) autoscaled";
                        display.PanelLayout = PlotterGraphPaneEx.LayoutMode.TILES_HOR;
                        display.DataSources[j].Length = 5800;
                        display.DataSources[j].AutoScaleY = true;
                        display.DataSources[j].SetDisplayRangeY(-300, 600);
                        display.DataSources[j].SetGridDistanceY(100);
                        //CalcSinusFunction_2(display.DataSources[j], j);
                        break;
                }
            }

            ApplyColorSchema();

            this.ResumeLayout();
            display.Refresh();

        }
        private void ApplyColorSchema()
        {
            Color[] cols = { Color.DarkRed, 
                                         Color.DarkSlateGray,
                                         Color.DarkCyan, 
                                         Color.DarkGreen, 
                                         Color.DarkBlue ,
                                         Color.DarkMagenta,                              
                                         Color.DeepPink };

            for (int j = 0; j < NumGraphs; j++)
            {
                display.DataSources[j].GraphColor = cols[j % 7];
            }

            display.BackgroundColorTop = Color.White;
            display.BackgroundColorBot = Color.LightGray;
            display.SolidGridColor = Color.LightGray;
            display.DashedGridColor = Color.LightGray;
        }

        public void buttonClick(string text1, string text2, string text3, string text4, string text5, string text6, string text7)
        {
            TN = double.Parse(text5);
            N = int.Parse(text6);
            NumGraphs = (int)TN;
            double T = double.Parse(text4);
            double TL = double.Parse(text7);
            PoohMathParser.MathExpression f = new PoohMathParser.MathExpression(text3);
            PoohMathParser.MathExpression u0 = new PoohMathParser.MathExpression(text2);
            PoohMathParser.MathExpression q = new PoohMathParser.MathExpression(text1);
            string u0text = text2;
            c = new Calulator(q, u0, f, TL, T, TN, N, text2, text3);
            CalcDataGraphs();

            display.Refresh();  
        }
    }
}