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
	/// Логика взаимодействия для SecondWindow.xaml
	/// </summary>
	public partial class SecondWindow : Window
	{
		private char[,] grid = new char[5, 5];
		//private Dictionary<ComboBox, int> tags = new Dictionary<ComboBox, int>();
		private Dictionary<int, ComboBox> tags = new Dictionary<int, ComboBox>();
		public SecondWindow()
		{
			InitializeComponent();
			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					grid[i, j] = '-';
				}
			}
			int tag = 0;
			foreach (var cell in this.myGrid.Children)
			{
				if (cell.GetType() == typeof(ComboBox))
				{
					tags.Add(tag++, (ComboBox)cell);
				}

			}

		}
		
		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			MainWindow mw = new MainWindow();
			this.Close();
			mw.Show();
		}

		private void MoveButton_Click(object sender, RoutedEventArgs e)
		{
			// x o x o x o
			// check if changed only one
			int changedCount = 0;
			int changedTag = 0;
			int tag = 0;
			foreach(var cell in this.myGrid.Children)
			{
				if (cell.GetType() == typeof(ComboBox))
				{
					if (grid[tag / 5, tag % 5] != ((ComboBox)cell).Text[0])
					{
						changedCount++;
						changedTag = tag;
						grid[tag / 5, tag % 5] = ((ComboBox)cell).Text[0];

					}
					tag++;
				}
			}
			char win = CheckWin();
			if (win == '-')
			{
				if (changedCount != 1 || grid[changedTag / 5, changedTag % 5] == 'o')
				{
					MessageBox.Show("Impossible move", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}
				else
				{
					myGrid.Children[changedTag].IsEnabled = false;
					Random rand = new Random();
					while(true)
					{
						tag = rand.Next(25);
						if (grid[tag / 5, tag % 5] == '-')
						{
							ComboBox cmb = tags[tag];
							grid[tag / 5, tag % 5] = 'o';
							cmb.SelectedIndex = 1;
							cmb.IsEnabled = false;
							win = CheckWin();
							break;
						}
					}
				}
			}
			if (win == 'x') { MessageBox.Show("x won", "Victory", MessageBoxButton.OK); ExitButton_Click(sender, e); }
			else if (win == 'o') { MessageBox.Show("o won", "Defeat", MessageBoxButton.OK); ExitButton_Click(sender, e); }
			else if (win == 'd') { MessageBox.Show("Draw", "Draw", MessageBoxButton.OK); ExitButton_Click(sender, e); }
			
		}
		private char CheckWin()
		{
			int i, j;
			bool gridFull = true;
			for(i = 0; i < 5; i++) // check if grid is filled
			{
				for(j = 0; j < 5; j++)
				{
					if(grid[i, j] == '-')
					{
						gridFull = false;
					}
				}
			}
			bool same = true;
			i = j = 0;
			while (i < 4 && j < 4) // check main diagonal
			{
				if(grid[i, j] != grid[i + 1, j + 1])
				{
					same = false;
					break;
				}
				i++; j++;
			}
			if (same) return grid[0, 0];
			i = 0; j = 4;
			while (i < 4 && j > 0) // check side diagonal
			{
				if (grid[i, j] != grid[i + 1, j - 1])
				{
					same = false;
					break;
				}
				i++; j--;
			}
			if (same) return grid[0, 4];

			for(i = 0; i < 5; i++) // check rows
			{
				same = true;
				for(j = 0; j < 4; j++)
				{
					if (grid[i, j] != grid[i, j + 1])
					{
						same = false;
					}

				}
				if (same) return grid[i, 0];
			}

			for (j = 0; j < 5; j++) // check columns
			{
				same = true;
				for (i = 0; i < 4; i++)
				{
					if (grid[i, j] != grid[i + 1, j])
					{
						same = false;
					}

				}
				if (same) return grid[0, j];
			}
			if (gridFull)
				return 'd'; // draw
			else return '-';
		}
	}
}
