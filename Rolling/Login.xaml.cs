using System;
using System.Collections.Generic;
using System.Data.OleDb;
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
using MaterialDesignThemes.Wpf;

namespace Rolling
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            DataContext = this;
            TxtUsername.Text = "Enter Username";
        }


        public bool IsDarkTheme { get; set; }
        private readonly PaletteHelper _paletteHelper = new PaletteHelper();


        public string DUsername { get; set; }
        public string DPassword { get; set; }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext == null)
            {
                return;
            }

            ((dynamic)DataContext).DPassword = ((PasswordBox)sender).Password;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void ToggleTheme(object sender, RoutedEventArgs e)
        {
            ITheme theme = _paletteHelper.GetTheme();
            if (IsDarkTheme = (theme.GetBaseTheme() == BaseTheme.Dark))
            {
                IsDarkTheme = false;
                theme.SetBaseTheme(Theme.Light);
            }
            else
            {
                IsDarkTheme = true;
                theme.SetBaseTheme(Theme.Dark);
            }

            _paletteHelper.SetTheme(theme);
        }

        private void ExitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void DoLogin(object sender, RoutedEventArgs routedEventArgs)
        {
            _ = Autorise();
            if(Autorise())
            {
                this.Hide();
                Menu menu = new Menu();
                menu.Show();
            }
            else
            {
                
                NewMessageBox nmb = new NewMessageBox();
                nmb.Show();
                nmb.ErrorText.Text = "User does not exist!";
            }

        }

        private bool Autorise()
        {
            string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                      Data Source=C:\Users\HomeUser\Desktop\wpf\Rolling\Rolling\RollingDB.mdb";
            OleDbConnection connection = new OleDbConnection(connectionString);
            connection.Open();
            string sql = "SELECT * FROM Accounts";
            OleDbCommand command = new OleDbCommand(sql, connection);
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (TxtUsername.Text == reader[0].ToString() && TxtPassword.Password == reader[1].ToString())
                {
                    return true;
                }
            }

            return false;
        }

        private void DoRegistartion(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Registration reg = new Registration();
            reg.Show();
        }
    }
}
