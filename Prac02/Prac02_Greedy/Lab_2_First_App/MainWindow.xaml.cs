using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Lab_2_First_App
{
    public partial class MainWindow : Window
    {
        static DispatcherTimer dT = new DispatcherTimer();
        static Random rand = new Random();
        static int Radius = 20;
        static int pointCount;
        static int numOfIter = 0;
        static int velocity;
        static double length = 0;
        static List<int> way = new List<int>();
        static List<int> visited = new List<int>();
        static Polygon myPolygon = new Polygon();
        static List<Line> lines = new List<Line>();
        static List<Ellipse> EllipseArray = new List <Ellipse>();
        static PointCollection pC = new PointCollection();

        public MainWindow()
        {

            InitializeComponent();
            InitPoints();
            //InitPolygon();

            dT.Tick += new EventHandler(OneStep);
            dT.Interval = new TimeSpan(0, 0, 0, 0, 1000);
        }

        private void InitPoints()
        {
            pC.Clear();
            lines.Clear();
            EllipseArray.Clear();

            for (int i = 0; i < pointCount; i++)
            {
                Point p = new Point();

                p.X = rand.Next(Radius, (int)(0.66*MainWin.Width)-3*Radius);
                p.Y = rand.Next(Radius, (int)(0.90*MainWin.Height-3*Radius));
                pC.Add(p);
            }

            for (int i = 0; i < pointCount; i++)
            { 
                Ellipse el = new Ellipse();

                el.StrokeThickness = 2;
                el.Height = el.Width = Radius;
                el.Stroke = Brushes.Black;
                el.Fill = Brushes.LightBlue;
				EllipseArray.Add(el);
			}
		}


        private void PlotPoints()
        {
            for (int i = 0; i < pointCount; i++)
            {
                Canvas.SetLeft(EllipseArray[i], pC[i].X - Radius/2);
                Canvas.SetTop(EllipseArray[i], pC[i].Y - Radius/2);
                MyCanvas.Children.Add(EllipseArray[i]);
            }
        }


        private void PlotWay()
        {
            /*MyCanvas.Children.Remove(myPolygon);

            PointCollection Points = new PointCollection();*/
            for (int i = 0; i < lines.Count; i++)
            {
                MyCanvas.Children.Add(lines[i]);
                //Points.Add(pC[BestWay[i]]);
            }
            /*myPolygon.Points = Points;
            MyCanvas.Children.Add(myPolygon);*/
        }

        private void StopStart_Click(object sender, RoutedEventArgs e)
        {
            if (dT.IsEnabled)
            {
                dT.Stop();
                NumElemCB.IsEnabled = true;
                VelCB.IsEnabled = true;

            }
            else
            {
                NumElemCB.IsEnabled = false;
                VelCB.IsEnabled = false;
                dT.Start();
                pointCount = GetComboBoxContentInt32(NumElemCB);
                velocity = GetComboBoxContentInt32(VelCB);
                //dT.IsEnabled = true;
                dT.Interval = new TimeSpan(0, 0, 0, 0, velocity);
                numOfIter = 0;
                MyCanvas.Children.Clear();
                InitPoints();
                PlotPoints();
                way.Clear();
                visited.Clear();
                way.Add(0);
                visited.Add(0);
            }
            
        }
        private Int32 GetComboBoxContentInt32(ComboBox cb) => Convert.ToInt32(((ComboBoxItem)cb.SelectedItem).Content);
        private double GetComboBoxContentDouble(ComboBox cb) => Convert.ToDouble(((ComboBoxItem)cb.SelectedItem).Content);



        private void OneStep(object sender, EventArgs e)
        {
            if(numOfIter < pointCount)
			{
                MyCanvas.Children.Clear();
                if (visited.Count < pointCount)//numOfIter != pointCount - 1)
                {
                    int minIndex = 0;
                    double minValue = double.MaxValue, current;
                    for (int i = 1; i < pointCount; i++)
                    {
                        current = dist(pC[way[way.Count - 1]], pC[i]);
                        if (current < minValue && !visited.Contains(i))
                        {
                            minValue = current;
                            minIndex = i;
                        }
                    }
                    lines.Add(new Line());
                    lines[lines.Count - 1].X1 = pC[way[way.Count-1]].X;
                    lines[lines.Count - 1].Y1 = pC[way[way.Count - 1]].Y;
                    lines[lines.Count - 1].X2 = pC[minIndex].X;
                    lines[lines.Count - 1].Y2 = pC[minIndex].Y;
                    lines[lines.Count - 1].StrokeThickness = 2;
                    lines[lines.Count - 1].Stroke = Brushes.Black;
                    way.Add(minIndex);
                    visited.Add(minIndex);
                    length += minValue;
                    //MessageBox.Show(String.Join(",", way));
                }
                else if (visited.Count == pointCount)
                {
                    
                    lines.Add(new Line());
                    lines[lines.Count - 1].X1 = pC[way[way.Count - 1]].X;
                    lines[lines.Count - 1].Y1 = pC[way[way.Count - 1]].Y;
                    lines[lines.Count - 1].X2 = pC[0].X;
                    lines[lines.Count - 1].Y2 = pC[0].Y;
                    lines[lines.Count - 1].StrokeThickness = 2;
                    lines[lines.Count - 1].Stroke = Brushes.Black;
                    //MessageBox.Show(String.Join(",", lines[4]));
                    way.Add(0);
                    length += dist(pC[way[way.Count - 1]], pC[0]);
                }
                PlotWay();
                PlotPoints();
                numOfIter++;
                CurrentIter.Content = Math.Round(length, 2).ToString() + "   " + numOfIter.ToString();
                if (numOfIter == pointCount)
                    MessageBox.Show("Пошук закінчений");
            }

        }
        private double dist(Point p1, Point p2) => Point.Subtract(p2, p1).Length;
        
		private void Pause_Click(object sender, RoutedEventArgs e)
		{
            dT.IsEnabled = dT.IsEnabled ^ true;
        }
	}
}