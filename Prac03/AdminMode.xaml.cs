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
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Prac03
{
	/// <summary>
	/// Логика взаимодействия для AdminMode.xaml
	/// </summary>
	public partial class AdminMode : Window
	{
		string connectionString = @"Data Source=LAPTOP-29IABVA8;Initial Catalog = Prac03; Integrated Security = True";
		SqlConnection sqlConn;
		SqlCommand cmd;
		SqlDataAdapter Data;
		DataTable dT = new DataTable(), dT1 = new DataTable();
		string strQ;
		int countTries;
		int userIndex;
		bool flag = true;
		public AdminMode()
		{
			InitializeComponent();
			//connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
			countTries = 0;
			userIndex = 0;
			MyGrid1.CanUserAddRows = false;
		}

		private void SignIn_Click(object sender, RoutedEventArgs e)
		{
			userIndex = 0;
			sqlConn = new SqlConnection(connectionString);
			sqlConn.Open();
			strQ = "SELECT MainTable.Password FROM MainTable WHERE Login='ADMIN';";
			dT1 = new DataTable();
			cmd = new SqlCommand(strQ, sqlConn);
			Data = new SqlDataAdapter(cmd);
			Data.Fill(dT1);
			if (AdminPass.Password == dT1.Rows[0][0].ToString())
			{
				MessageBox.Show("Succesful sign in");
				foreach (Control elem in adminGrid.Children)
					elem.IsEnabled = true;
				UpdateDataTable();
			}
			else
			{
				countTries++;
				MessageBox.Show($"Wrong password! Try number {countTries}");
				if (countTries == 3)
				{
					Application.Current.Shutdown();
				}
			}
			AdminPass.Clear();
		}
		private void UpdateDataTable()
		{
			sqlConn = new SqlConnection(connectionString);
			sqlConn.Open();
			strQ = "SELECT Name, Surname, Login, Status, Restriction FROM MainTable;";
			cmd = new SqlCommand(strQ, sqlConn);
			Data = new SqlDataAdapter(cmd);
			dT = new DataTable();
			Data.Fill(dT);
			MyGrid1.ItemsSource = dT.DefaultView;
			sqlConn.Close();

			UserCB.Items.Clear();
			for (int i = 0; i < dT.Rows.Count; i++)
				UserCB.Items.Add(dT.Rows[i][2]);
			UserCB.SelectedIndex = userIndex;
			flag = true;
			UpdateWindow();
		}
		void UpdateWindow()
		{
			Name.Content = dT.Rows[userIndex][0].ToString();
			Surname.Content = dT.Rows[userIndex][1].ToString();
			Login.Content = dT.Rows[userIndex][2].ToString();
			Status.Content = dT.Rows[userIndex][3].ToString();
			Restriction.Content = dT.Rows[userIndex][4].ToString();
			Activity.IsChecked = (dT.Rows[Math.Max(UserCB.SelectedIndex, 0)][3].ToString() == "True");
			Restrict.IsChecked = (dT.Rows[Math.Max(UserCB.SelectedIndex, 0)][4].ToString() == "True");
			if (UserCB.SelectedIndex != -1 && UserCB.SelectedValue.ToString() == "ADMIN")
				Activity.IsEnabled = false;
			else
				Activity.IsEnabled = true;
		}
		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			MainWindow mw = new MainWindow();
			Hide();
			mw.Show();
		}


		private void AdminMode_Closed(object sender, EventArgs e)
		{
			MainWindow mw = new MainWindow();
			Hide();
			mw.Show();
		}

		private void LogOut_Click(object sender, RoutedEventArgs e)
		{
			OldPass.Password = "";
			NewPass1.Password = "";
			NewPass2.Password = "";
			dT.Clear();
			Login.Content = "";
			Status.Content = "";
			Name.Content = "";
			Surname.Content = "";
			userIndex = 0;
			flag = false;
			UserCB.SelectedIndex = -1;
			MyGrid1.ItemsSource = dT.DefaultView;
			AdminPass.Password = "";
			foreach (Control elem in adminGrid.Children)
				elem.IsEnabled = false;
			AdminPass.IsEnabled = true;
			CloseWindow.IsEnabled = true;
			SignIn.IsEnabled = true;

		}
		bool CorrectPass(string login, string pass)
		{
			sqlConn = new SqlConnection(connectionString);
			sqlConn.Open();
			strQ = "SELECT Password FROM MainTable WHERE Login ='" + login + "';";
			cmd = new SqlCommand(strQ, sqlConn);
			Data = new SqlDataAdapter(cmd);
			dT1 = new DataTable();
			Data.Fill(dT1);
			sqlConn.Close();
			return pass == dT1.Rows[0][0].ToString();

		}
		bool IsRestricted(string login)
		{
			sqlConn = new SqlConnection(connectionString);
			sqlConn.Open();
			strQ = "SELECT Restriction FROM MainTable WHERE Login ='" + login + "';";
			cmd = new SqlCommand(strQ, sqlConn);
			Data = new SqlDataAdapter(cmd);
			dT1 = new DataTable();
			Data.Fill(dT1);
			sqlConn.Close();
			return (bool)dT1.Rows[0][0];
		}
		bool CheckRestricted(string pass)
		{
			byte Count1, Count2, Count3;
			byte length = (byte)pass.Length;
			Count1 = Count2 = Count3 = 0;
			for (byte i = 0; i < length; i++)
			{
				if (pass[i] >= 'A' && pass[i] <= 'Z')
					Count1++;
				if (pass[i] >= 'a' && pass[i] <= 'z')
					Count2++;
				if ((pass[i] >= '0' && pass[i] <= '9') || pass[i] == '+' || pass[i] == '-' || pass[i] == '*' || pass[i] == '/')
					Count3++;
			}
			return Count1 * Count2 * Count3 != 0;
		}

		private void Previous_Click(object sender, RoutedEventArgs e)
		{
			if (userIndex > 0)
			{
				userIndex--;
				UpdateWindow();
			}
		}

		private void Next_Click(object sender, RoutedEventArgs e)
		{
			if (userIndex < dT.Rows.Count - 1)
			{
				userIndex++;
				UpdateWindow();
			}
		}

		private void AddUser_Click(object sender, RoutedEventArgs e)
		{
			sqlConn = new SqlConnection(connectionString);
			sqlConn.Open();
			string UserLogin = NewUser.Text;
			try
			{
				if (sqlConn.State == System.Data.ConnectionState.Open)
				{
					strQ = "INSERT INTO MainTable (Name, Surname, Login, Status, Restriction) values('', '', '" + UserLogin + "', 1, 0); ";
					cmd = new SqlCommand(strQ, sqlConn);
					cmd.ExecuteNonQuery();
				}
				UpdateDataTable();
			}
			catch
			{
				MessageBox.Show("User wasn't added");
			}
			sqlConn.Close();
		}

		private void SetChanges_Click(object sender, RoutedEventArgs e)
		{
			sqlConn = new SqlConnection(connectionString);
			sqlConn.Open();
			string status = Activity.IsChecked.ToString();
			string restriction = Restrict.IsChecked.ToString();
			string login = (string)UserCB.SelectedValue;
			if (status == "True")
				status = "1";
			else
				status = "0";
			if (restriction == "True")
				restriction = "1";
			else
				restriction = "0";
			strQ = "UPDATE MainTable Set Status = " + status + ", Restriction = " + restriction + " WHERE Login = '" + login + "';";
			cmd = new SqlCommand(strQ, sqlConn);
			cmd.ExecuteNonQuery();
			sqlConn.Close();
			UpdateDataTable();
		}

		private void ChangedUser(object sender, SelectionChangedEventArgs e)
		{
			if (flag)
				UpdateWindow();
		}

		private void UpdatePass_Click(object sender, RoutedEventArgs e)
		{
			sqlConn = new SqlConnection(connectionString);
			//sqlConn.Open();
			string StrOldPass = OldPass.Password.ToString();
			string StrNewPass1 = NewPass1.Password.ToString();
			string StrNewPass2 = NewPass2.Password.ToString();
			if (CorrectPass("ADMIN", StrOldPass) && (StrNewPass1 != "") && (StrNewPass1 == StrNewPass2))
			{
				if ((IsRestricted("ADMIN") && CheckRestricted(StrNewPass1)) || !IsRestricted("ADMIN"))
				{
					sqlConn.Open();
					if (sqlConn.State == System.Data.ConnectionState.Open)
					{
						strQ = "UPDATE MainTable SET Password ='" + StrNewPass1 + "'WHERE Login = 'ADMIN'; ";
						cmd = new SqlCommand(strQ, sqlConn);
						cmd.ExecuteNonQuery();
						MessageBox.Show("Password has been changed");
						sqlConn.Close();
						LogOut_Click(sender, e);
						return;
					}
				}
				else MessageBox.Show("Password must contain at least one small letter, one capital letter and an arithmetic character");
			}
			else
				MessageBox.Show("Wrong password");
		}
	}
}

