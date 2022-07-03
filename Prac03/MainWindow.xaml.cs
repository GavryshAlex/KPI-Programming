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

namespace Prac03
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
		private void AdminMode_Click(object sender, RoutedEventArgs e)
		{
			AdminMode adminFormWPF = new AdminMode();
			Hide();
			adminFormWPF.Show();
		}
		private void UserMode_Click(object sender, RoutedEventArgs e)
		{
			UserMode userFormWPF = new UserMode();
			Hide();
			userFormWPF.Show();
		}
		private void UserMode_Copy_Click(object sender, RoutedEventArgs e)
		{
			System.Windows.Application.Current.Shutdown();
		}
		private void AboutDev_Click(object sender, RoutedEventArgs e)
		{
			DevWindow devWindow = new DevWindow();
			devWindow.Show();
		}
	}
}
