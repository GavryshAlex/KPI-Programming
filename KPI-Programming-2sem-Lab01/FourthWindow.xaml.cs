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

namespace KPI_Programming_2sem_Lab01
{
	/// <summary>
	/// Логика взаимодействия для FourthWindow.xaml
	/// </summary>
	public partial class FourthWindow : Window
	{
		public FourthWindow()
		{
			InitializeComponent();
		}

		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			MainWindow mw = new MainWindow();
			this.Close();
			mw.Show();
		}
	}
}
