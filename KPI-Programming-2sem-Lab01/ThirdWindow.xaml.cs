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
	/// Логика взаимодействия для ThirdWindow.xaml
	/// </summary>
	public partial class ThirdWindow : Window
	{
		private double First = double.NaN, Second = double.NaN, Result = double.NaN;
		private string op = "";
		private bool changeop = false;

		public ThirdWindow()
		{
			InitializeComponent();
		}

		private void BtnSign_Click(object sender, RoutedEventArgs e)
		{
			Button btn = (Button)e.Source;
			string btnContent = (string)btn.Content,
			       labelMainContent = (string)LabelMain.Content,
				   labelSideContent = (string)LabelSide.Content;
			if (changeop)
			{
				op = "" + labelSideContent[labelSideContent.Length - 1];
				First = Double.Parse(labelSideContent.Remove(labelSideContent.Length - 1, 1));
				Second = Double.Parse(labelMainContent);
				BtnEqual_Click(sender, e);
				LabelSide.Content = Result.ToString() + btnContent;
			}
			else First = Double.Parse((string)LabelMain.Content);
			if (btnContent == "+")
				op = "+";
			else if (btnContent == "-")
				op = "-";
			else if (btnContent == "×")
				op = "*";
			else if (btnContent == "÷")
				op = "/";
			LabelSide.Content = LabelMain.Content + op;
			LabelMain.Content = "0";
			changeop = true;
		}

		private void BtnEqual_Click(object sender, RoutedEventArgs e)
		{
			if (changeop) Second = Double.Parse((string)LabelMain.Content);
			string first, second;
			first = First.ToString();
			if (Second >= 0)
				second = Second.ToString();
			else
				second = "(" + Second.ToString() + ")";
			if (op == "+")
			{
				Result = First + Second;
				LabelMain.Content = Result.ToString();
				LabelSide.Content = first + "+" + second + "=";

			}
			else if (op == "-")
			{
				Result = First - Second;
				LabelMain.Content = Result.ToString();
				LabelSide.Content = first + "-" + second + "=";

			}
			else if (op == "*")
			{
				Result = First * Second;
				LabelMain.Content = Result.ToString();
				LabelSide.Content = first + "*" + second + "=";
			}
			else if (op == "/")
			{
				LabelSide.Content = first + "/" + second + "=";
				if (Second == 0)
				{
					LabelSide.Content = "Cannot be divided by zero";
					LabelMain.Content = "0";
				}
				else
				{
					Result = First / Second;
					LabelMain.Content = Result.ToString();
				}

			}
			changeop = false;
		}

		private void BtnChangeSign_Click(object sender, RoutedEventArgs e)
		{
			string str = (string)LabelMain.Content;
			if (str.Contains("-"))
			{
				LabelMain.Content = str.Remove(0, 1);
			}
			else
			{
				if (str != "0")
					LabelMain.Content = "-" + str;
			}
		}

		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			MainWindow mw = new MainWindow();
			this.Close();
			mw.Show();
		}

		private void BtnNumberDot_Click(object sender, RoutedEventArgs e)
		{
			string str = (string)LabelMain.Content;
			if (str.EndsWith("=")) BtnClear_Click(sender, e);
			Button btn = (Button)e.Source;
			if ((string)btn.Content == ".")
			{
				if(!str.Contains("."))
				{
					LabelMain.Content += ".";
					return;
				}
					
			}
			else
			{
				if (str != "0")
					LabelMain.Content += (string)btn.Content;
				else
					LabelMain.Content = (string)btn.Content;
			}
				
		}

		private void BtnClear_Click(object sender, RoutedEventArgs e)
		{
			LabelMain.Content = "0";
			LabelSide.Content = "";
			First = Second = Result = double.NaN;
			changeop = false;
		}
		private void BtnBackspace_Click(object sender, RoutedEventArgs e)
		{
			string str = (string)LabelMain.Content;
			if (str.Contains('E')) return;
			if (str.Length > 0)
			{
				str = str.Remove(str.Length - 1, 1);
				LabelMain.Content = str;
			}
			if (str == "")
			{
				LabelMain.Content = "0";
			}
		}

	}
}
