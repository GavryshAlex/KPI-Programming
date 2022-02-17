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
using System.IO;
using System.Globalization;

namespace KPI_Programming_2sem_Lab01
{
	/// <summary>
	/// Логика взаимодействия для FirstWindow.xaml
	/// </summary>
	public partial class FirstWindow : Window
	{
		public FirstWindow()
		{
			InitializeComponent();
		}

		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			MainWindow mw = new MainWindow();
			this.Close();
			mw.Show();
		}

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (ComboBox1.Text == "Видалити") // now add
			{
				PIB.IsEnabled = PIBLabel.IsEnabled = BirthDate.IsEnabled = BirthDateLabel.IsEnabled = Address.IsEnabled = AddressLabel.IsEnabled = true;

			}
			else if (ComboBox1.Text == "Додати") // now delete
			{
				PIB.IsEnabled = PIBLabel.IsEnabled = BirthDate.IsEnabled = BirthDateLabel.IsEnabled = Address.IsEnabled = AddressLabel.IsEnabled = false;
				
			}
		}

		private bool CheckCorrect()
		{
			if (!long.TryParse(ID.Text, out long n))
			{
				return false;
			}
			string[] splitPIB = PIB.Text.Split(' ');
			if (splitPIB.Length == 3)
			{
				for (int i = 0; i < PIB.Text.Length; i++)
				{
					if (!(char.IsLetter(PIB.Text[i]) || PIB.Text[i] == ' '))
						return false;
				}
			}
			else return false;
			if (!DateTime.TryParse(BirthDate.Text, out DateTime dateValue))
				return false;
			return true;
		}

		private void ProcessButton_Click(object sender, RoutedEventArgs e)
		{
			string currnetDirectory = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.Length - 9);
			if (ComboBox1.Text == "Додати")
			{
				try
				{
					StreamReader sr = new StreamReader(currnetDirectory + @"studentslist.txt");
					string[] currentStudent = new string[4];
					while (!sr.EndOfStream)
					{
						currentStudent = sr.ReadLine().Split(';');
						if (currentStudent[0] == ID.Text)
						{
							MessageBox.Show("Student already exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
							sr.Close();
							return;
						}
					}
					sr.Close();
					if (!CheckCorrect())
					{
						MessageBox.Show("Error adding a new student", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
						return;
					}
					StreamWriter sw = new StreamWriter(currnetDirectory + @"studentslist.txt", true);
					sw.Write($"\r\n{ID.Text};{PIB.Text};{BirthDate.Text};{Address.Text}");
					sw.Close();
					MessageBox.Show("Student successfully added", "Success", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}
				catch
				{
					MessageBox.Show("Error adding a new student", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}
			}
			else if (ComboBox1.Text == "Видалити")
			{
				try
				{
					bool exists = false;
					StreamReader sr = new StreamReader(currnetDirectory + @"studentslist.txt");
					string[] currentStudent = new string[4];
					string currentstr, updatedList = "";
					while (!sr.EndOfStream)
					{
						currentstr = sr.ReadLine();
						currentStudent = currentstr.Split(';');
						if (currentStudent[0] == ID.Text)
						{
							exists = true;
							continue;
						}
						updatedList += currentstr + "\n";
					}
					sr.Close();
					File.WriteAllText(currnetDirectory + @"studentslist.txt", updatedList);
					if(exists == false) MessageBox.Show("Student doesn't exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					else MessageBox.Show("Student successfully deleted", "Success", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}
				catch
				{
					MessageBox.Show("Error deleting a new student", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}
			}


		}
	}
}
