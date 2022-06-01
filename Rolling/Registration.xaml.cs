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
    public partial class Registration : Window
    {
        public Registration()
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

            ((dynamic) DataContext).DPassword = ((PasswordBox) sender).Password;
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

        private void DoRegistation(object sender, RoutedEventArgs e)
        {
            string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                      Data Source=C:\Users\HomeUser\Desktop\wpf\Rolling\Rolling\RollingDB.mdb";
            OleDbConnection connection = new OleDbConnection(connectionString);
            connection.Open();
            string sql = "INSERT INTO Accounts (Account, [Password]) VALUES ('" + TxtUsername.Text + "', '" + TxtPassword.Password + "')";
            OleDbCommand command = new OleDbCommand(sql, connection);
            command.ExecuteNonQuery();
            Login afterReg = new Login();
            afterReg.Show();
            NewMessageBox alr = new NewMessageBox();
            alr.Show();
            alr.ErrorText.Text = "You were registered successfully!";
        } 
    }
}

