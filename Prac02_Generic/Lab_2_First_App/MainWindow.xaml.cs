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
        static int sizeOfPop;
        static double chanceMut;
        static int numOfIter;
        static int velocity;
        static List<List<int>> population = new List<List<int>>();
        static Polygon myPolygon = new Polygon();
        static List<Ellipse> EllipseArray = new List <Ellipse>();
        static PointCollection pC = new PointCollection();

        public MainWindow()
        {

            InitializeComponent();
            InitPoints();
            InitPolygon();

            dT.Tick += new EventHandler(OneStep);
            dT.Interval = new TimeSpan(0, 0, 0, 0, 1);
        }

        private void InitPoints()
        {
            pC.Clear();
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


        private void InitPolygon()
        {
            myPolygon.Stroke = System.Windows.Media.Brushes.Black;
            myPolygon.StrokeThickness = 2;
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

        private List<int> CreateChromosome()
        {
            List<int> chromosome = new List<int>();
            for (int i = 0; i < pointCount; i++)
                chromosome.Add(i);
            Shuffle(chromosome);
            return chromosome;
        }

        private void Shuffle(List<int> list)
        {
            
            for (int k = 0; k < list.Count; k++)
            {
                int i = rand.Next(list.Count);
                int j = rand.Next(list.Count);
                int temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }


        private void PlotWay(List<int> BestWay)
        {
            MyCanvas.Children.Remove(myPolygon);

            PointCollection Points = new PointCollection();
            for (int i = 0; i < BestWay.Count; i++)
            {

                Points.Add(pC[BestWay[i]]);
            }
            myPolygon.Points = Points;
            MyCanvas.Children.Add(myPolygon);
        }

        private void StopStart_Click(object sender, RoutedEventArgs e)
        {
            if (dT.IsEnabled)
            {
                dT.Stop();
                NumElemCB.IsEnabled = true;
                VelCB.IsEnabled = true;
                SizeOfPop.IsEnabled = true;
                NumOfIter.IsEnabled = true;
                ChanceMut.IsEnabled = true;

            }
            else
            {
                NumElemCB.IsEnabled = false;
                VelCB.IsEnabled = false;
                SizeOfPop.IsEnabled = false;
                NumOfIter.IsEnabled = false;
                ChanceMut.IsEnabled = false;
                dT.Start();
                pointCount = GetComboBoxContentInt32(NumElemCB);
                sizeOfPop = GetComboBoxContentInt32(SizeOfPop);
                chanceMut = GetComboBoxContentDouble(ChanceMut);
                numOfIter = GetComboBoxContentInt32(NumOfIter);
                velocity = GetComboBoxContentInt32(VelCB);
                //dT.IsEnabled = true;
                dT.Interval = new TimeSpan(0, 0, 0, 0, velocity);
                MyCanvas.Children.Clear();
                InitPoints();
                PlotPoints();
                population.Clear();
                for (int i = 0; i < sizeOfPop; i++)
                    population.Add(CreateChromosome());

            }
            
        }
        private Int32 GetComboBoxContentInt32(ComboBox cb) => Convert.ToInt32(((ComboBoxItem)cb.SelectedItem).Content);
        private double GetComboBoxContentDouble(ComboBox cb) => Convert.ToDouble(((ComboBoxItem)cb.SelectedItem).Content);
        private void unique(List<int> l)
        {
            List<int> cop = new List<int>();
            for (int i = 0; i < l.Count; i++)
            {
                if (!cop.Contains(l[i]))
                {
                    cop.Add(l[i]);
                }
            }
            l.Clear();
            for (int i = 0; i < cop.Count; i++)
                l.Add(cop[i]);
        }

        private void Mutation(List<int> chromosome)
		{
            int i1 = rand.Next(chromosome.Count);
            int i2 = rand.Next(chromosome.Count);
            if (i2 < i1)
            {
                int temp = i1;
                i1 = i2;
                i1 = temp;
            }
            for (int i = i1; i < i1 + (i2 - i1 + 1) / 2; i++)
            {
                int temp = chromosome[i];
                chromosome[i] = chromosome[i2 - (i - i1)];
                chromosome[i2 - (i - i1)] = temp;
            }
            /*int i = rand.Next(chromosome.Count);
            int j = rand.Next(chromosome.Count);
            int temp = chromosome[i];
            chromosome[i] = chromosome[j];
            chromosome[j] = temp;*/
        }

        private void MakeChild(List<int> p1, List<int> p2)
        {
            int crossover = rand.Next(pointCount);
            List<int> tmp1 = new List<int>();
            List<int> tmp2 = new List<int>();
            for (int i = 0; i <= crossover; i++)
            {
                tmp1.Add(p1[i]);
                tmp2.Add(p2[i]);
            }
            for (int i = crossover + 1; i < p1.Count; i++)
            {
                tmp1.Add(p2[i]);
                tmp2.Add(p1[i]);
            }
            List<int> child1 = new List<int>();
            List<int> child2 = new List<int>();
            for (int i = 0; i < tmp1.Count; i++)
            {
                child1.Add(tmp1[i]);
                child2.Add(tmp2[i]);
            }
            for (int i = 0; i < tmp2.Count; i++)
            {
                child1.Add(tmp2[i]);
                child2.Add(tmp1[i]);
            }
            double doublerand = rand.NextDouble();
            unique(child1);
            unique(child2);
            doublerand = rand.NextDouble();
            
            if (doublerand <= chanceMut)
                Mutation(child1);
            doublerand = rand.NextDouble();
            if (doublerand <= chanceMut)
                Mutation(child2);


            population.Add(child1);
            population.Add(child2);
        }

        private void OneStep(object sender, EventArgs e)
        {
            if (numOfIter != 0)
            {
                while (population.Count < 2 * sizeOfPop)
                {
                    int i1 = rand.Next(sizeOfPop);
                    int i2 = rand.Next(sizeOfPop);
                    MakeChild(population[i1], population[i2]);
                }
                List<Tuple<double, List<int>>> choose = new List<Tuple<double, List<int>>>();
                for (int i = 0; i < population.Count; i++)
                {
                    choose.Add(new Tuple<double, List<int>>(len(population[i]), population[i]));

                }
                choose.Sort((x, y) => x.Item1.CompareTo(y.Item1));
                population.Clear();
                for (int i = 0; i < sizeOfPop; i++)
                {
                    population.Add(new List<int>());
                    for(int j = 0; j < pointCount; j++)
					{
                        population[i].Add(choose[i].Item2[j]);
					}
                }
                PlotWay(population[0]);
                numOfIter--;
                CurrentIter.Content = Math.Round(len(population[0]), 2).ToString() + "   " + (GetComboBoxContentInt32(NumOfIter) - numOfIter).ToString();
                if (numOfIter == 0)
                    MessageBox.Show("Пошук закінчений");

            }
        }
        private double len(List<int> curPop)
        {
            double res = 0;
            for (int i = 1; i < pointCount; i++)
            {
                res += dist(pC[curPop[i]], pC[curPop[i - 1]]);
            }
            res += dist(pC[curPop[pointCount - 1]], pC[curPop[0]]);
            return res;
        }
        private double dist(Point p1, Point p2) => Point.Subtract(p2, p1).Length;
        
		private void Pause_Click(object sender, RoutedEventArgs e)
		{
            dT.IsEnabled = dT.IsEnabled ^ true;
        }
	}
}