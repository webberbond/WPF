using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace supply
{
    public partial class Registration : Window
    {
        private readonly SqlConnection sqlConnection =
            new SqlConnection(@"Data Source=WEBBER\SQLEXPRESS;Initial Catalog=supplier;Integrated Security=True");
        public Registration()
        {
            InitializeComponent();
        }

        private void Reg_Click(object sender, RoutedEventArgs e)
        {
            var login = txtLog.Text;
            var password = pwdPass.Password;

            string querystr = $"INSERT INTO register (login_user, password_user) VALUES('{login}', '{password}')";
            SqlCommand command = new SqlCommand(querystr, sqlConnection);
            sqlConnection.Open();

            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("You have been registered successfully");
                Login frm_login = new Login();
                this.Hide();
                frm_login.Show();
            }
            else
            {
                MessageBox.Show("You were not registered");
            }

            if (checkuser())
            {
                return;
            }

            sqlConnection.Open();
        }

        private Boolean checkuser()
        {
            var loginUser = txtLog.Text;
            var passUser = pwdPass.Password;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            string querystr = $"SELECT id_user, login_user, password_user FROM register WHERE login_user = '{loginUser}' AND password_user = '{passUser}'";
            SqlCommand command = new SqlCommand(querystr, sqlConnection);

            adapter.SelectCommand = command;
            adapter.Fill(table);
            return true;
        }
    }
}