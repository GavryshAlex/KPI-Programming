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

namespace KPI_Programming_2sem_Lab01
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			System.Windows.Application.Current.Shutdown();
		}

		private void FirstButton_Click(object sender, RoutedEventArgs e)
		{
			FirstWindow first = new FirstWindow();
			//this.Hide();
			this.Close();
			first.Show();
		}

		private void SecondButton_Click(object sender, RoutedEventArgs e)
		{
			SecondWindow second = new SecondWindow();
			this.Hide();
			second.Show();
		}

		private void ThridButton_Click(object sender, RoutedEventArgs e)
		{
			ThirdWindow third = new ThirdWindow();
			this.Hide();
			third.Show();
		}

		private void FourthButton_Click(object sender, RoutedEventArgs e)
		{
			FourthWindow fourth = new FourthWindow();
			this.Hide();
			fourth.Show();
		}
	}
}
