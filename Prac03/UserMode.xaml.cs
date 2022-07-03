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
	/// Логика взаимодействия для UserMode.xaml
	/// </summary>
	public partial class UserMode : Window
	{
        string connectionString = @"Data Source=LAPTOP-29IABVA8;Initial Catalog = Prac03; Integrated Security = True";
        SqlConnection sqlConn;
        SqlCommand cmd;
        SqlDataAdapter Data;
        DataTable dT, dT1;
        string strQ;
        int countTries;
        string login_l;

        bool flag = true;
        public UserMode()
		{
			InitializeComponent();
		}
       


        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            Hide();
            mw.Show();
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            sqlConn = new SqlConnection(connectionString);
            sqlConn.Open();
            login_l = Login_l.Text;
            string pass = Pass_l.Password;
            strQ = "SELECT Password, Status FROM MainTable WHERE Login = '" + login_l + "';";
            cmd = new SqlCommand(strQ, sqlConn);
            dT = new DataTable();
            Data = new SqlDataAdapter(cmd);
            Data.Fill(dT);
            if (dT.Rows.Count == 0)
            {
                MessageBox.Show("User does not exist");
                return;
            }
            if (dT.Rows[0][1].ToString() == "False")
            {
                MessageBox.Show("User is blocked");
                Login_l.Text = "";
                Pass_l.Password = "";
                return;
            }


            if (dT.Rows[0][0].ToString() == pass)
            {
                Login_l.Text = "";
                MessageBox.Show("Succesful sign in");
                foreach (Control el in userGrid.Children)
                    el.IsEnabled = true;
                countTries = 0;

            }
            else
            {
                countTries++;
                MessageBox.Show($"Wrong password! Try number {countTries}");
                if (countTries == 3)
                    Application.Current.Shutdown();
            }
            sqlConn.Close();

            Pass_l.Password = "";
        }

        private void Login_l_TextChanged(object sender, TextChangedEventArgs e)
        {
            countTries = 0;
        }
        bool IsRestricted(string login)
        {
            sqlConn = new SqlConnection(connectionString);
            sqlConn.Open();
            strQ = "SELECT Restriction FROM MainTable WHERE Login ='" + login + "';";
            cmd = new SqlCommand(strQ, sqlConn);
            Data = new SqlDataAdapter(cmd);
            dT = new DataTable();
            Data.Fill(dT);
            sqlConn.Close();
            return (bool)dT.Rows[0][0];
        }
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            string Name = Name_l.Text;
            string Surname = Surname_l.Text;
            string pass1 = NewPass1.Password;
            string pass2 = NewPass2.Password;
            if (pass1 != pass2)
            {
                MessageBox.Show("Паролі не співпадають");
                return;
            }
            if (IsRestricted(login_l) && !CheckRestricted(pass1))
            {
                MessageBox.Show("Password must contain at least one small letter, one capital letter and an arithmetic character");
                return;
            }
            sqlConn = new SqlConnection(connectionString);
            sqlConn.Open();
            strQ = "UPDATE MainTable Set Name = '" + Name + "'," + " Surname = '" + Surname + "', " + "Password = '" + pass1 + "' WHERE Login = '" + login_l + "';";
            cmd = new SqlCommand(strQ, sqlConn);
            cmd.ExecuteNonQuery();
            sqlConn.Close();
            Name_l.Text = "";
            Surname_l.Text = "";
            NewPass1.Password = "";
            NewPass2.Password = "";
            MessageBox.Show("Successful update");
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
        private void Registr_Click(object sender, RoutedEventArgs e)
        {
            string Name = Name_r.Text;
            string Surname = Surname_r.Text;
            string login = Login_r.Text;
            string pass1 = Pass_r.Password;
            string pass2 = Pass1_r.Password;
            if(CheckLoginExist(login))
			{
                MessageBox.Show("User already exists or empty login");
                return;
            }
            if (pass1 != pass2)
            {
                MessageBox.Show("Passwords are not the same");
                return;
            }
            if (!CheckRestricted(pass1))
            {
                MessageBox.Show("Password must contain at least one small letter, one capital letter and an arithmetic character");
                return;
            }
            sqlConn = new SqlConnection(connectionString);
            sqlConn.Open();
            string SQLQuery = "INSERT INTO MainTable (Name, Surname, Login, Password, Status, Restriction) VALUES('" + Name + "', '" + Surname + "', '" + login + "', '" + pass1 + "', 1, " + "1);";
            cmd = new SqlCommand(SQLQuery, sqlConn);
            cmd.ExecuteNonQuery();
            sqlConn.Close();
            MessageBox.Show("Successful sign up");
        }

        private bool CheckLoginExist(string login)
		{
            sqlConn = new SqlConnection(connectionString);
            sqlConn.Open();
            strQ = "SELECT Login FROM MainTable WHERE Login ='" + login + "';";
            cmd = new SqlCommand(strQ, sqlConn);
            Data = new SqlDataAdapter(cmd);
            dT = new DataTable();
            Data.Fill(dT);
            sqlConn.Close();
			try
			{
				return (string)dT.Rows[0][0] == login;
			}
			catch
			{
                return false;
			}
		}


		private void SignOut_Click(object sender, RoutedEventArgs e)
		{
            Login_l.Text = "";
            Pass_l.Password = "";
            Name_l.Text = "";
            Surname_l.Text = "";
            NewPass1.Password = "";
            NewPass2.Password = "";
            dT.Clear();
            login_l = "";
            Update.IsEnabled = false;
        }

		private void Window_Closed(object sender, EventArgs e)
        {
            MainWindow mw = new MainWindow();
            Hide();
            mw.Show();
        }
    }
}
