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

namespace Rolling
{
    public partial class Catalog : Window
    {
        readonly string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                  Data Source=C:\Users\HomeUser\Desktop\wpf\Rolling\Rolling\RollingDB.mdb";
        private readonly OleDbConnection myConnection;
        private readonly List<Product> _item = new List<Product>();
        private readonly List<Product> _chain = new List<Product>();
        private readonly List<Product> _list = new List<Product>();
        private readonly List<Product> _abesItem = new List<Product>();
        private readonly List<Product> _catodAnod = new List<Product>();
        private readonly List<Product> _tubes = new List<Product>();
        private readonly List<Product> _wires = new List<Product>();
        private readonly List<Product> _filter = new List<Product>();

        public Catalog()
        {
            InitializeComponent();
            myConnection = new OleDbConnection(connectionString);
            myConnection.Open();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillDataFromDb(_item);
            FillChainDataFromDb(_chain);
            FillListDataFromDb(_list);
            FillCatodDataFromDb(_catodAnod);
            FillAbesDataFromDb(_abesItem);
            FillTubesDataFromDb(_tubes);
            FillWiresDataFromDb(_wires);

            NameData.ItemsSource = _item;
            ChainData.ItemsSource = _chain;
            ListData.ItemsSource = _list;
            AbsCementData.ItemsSource = _abesItem;
            CatodData.ItemsSource = _catodAnod;
            TubeData.ItemsSource = _tubes;
            ProvData.ItemsSource = _wires;
        }
        private void NameData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Product selectedItem = (Product)NameData.SelectedItem;
            SearchBlock.Text = Equals(NameData.ItemsSource, _item) ? selectedItem.Name : "";
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string itemName = SearchBlock.Text;
            string sql = "SELECT * FROM Stock WHERE Name = '" + itemName + "'";
            OleDbCommand command = new OleDbCommand(sql, myConnection);
            OleDbDataReader reader = command.ExecuteReader();
            NewMessageBox nmb = new NewMessageBox();
            while (reader.Read())
            {
                string n = reader[1].ToString();
                double p = Convert.ToDouble(reader[3]);
                double pt = Convert.ToDouble(reader[4]);
                int a = Convert.ToInt32(reader[5]);
                _filter.Add(new Product(n, p, pt, a));
            }
            if (_filter.Count == 0)
            {
                nmb.Show();
                nmb.ErrorText.Text = "The requested data was not found. Check the entered data.";
            }
            else
            {
                NameData.ItemsSource = _filter;
            }
        }
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            NameData.ItemsSource = _item;
            _filter.Clear();
            SearchBlock.Text = "";
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #region Заполнение списков данных
        static void FillDataFromDb(List<Product> item)
        {
            OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;
                                      Data Source=C:\Users\HomeUser\Desktop\wpf\Rolling\Rolling\RollingDB.mdb");
            connection.Open();
            string sql = "SELECT * FROM Stock";
            OleDbCommand command = new OleDbCommand(sql, connection);
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string n = reader[1].ToString();
                double p = Convert.ToDouble(reader[3]);
                double pt = Convert.ToDouble(reader[4]);
                int a = Convert.ToInt32(reader[5]);
                item.Add(new Product(n, p, pt, a));
            }
        }
        static void FillChainDataFromDb(List<Product> chain)
        {
            OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;
                                      Data Source=C:\Users\HomeUser\Desktop\wpf\Rolling\Rolling\RollingDB.mdb");
            connection.Open();
            string sql = "SELECT * FROM Stock WHERE Nomenclature = 'Цепь'";
            OleDbCommand command = new OleDbCommand(sql, connection);
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string n = reader[1].ToString();
                double p = Convert.ToDouble(reader[3]);
                double pt = Convert.ToDouble(reader[4]);
                int a = Convert.ToInt32(reader[5]);
                chain.Add(new Product(n, p, pt, a));
            }
        }
        static void FillListDataFromDb(List<Product> list)
        {
            OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;
                                      Data Source=C:\Users\HomeUser\Desktop\wpf\Rolling\Rolling\RollingDB.mdb");
            connection.Open();
            string sql = "SELECT * FROM Stock WHERE Nomenclature = 'Лист'";
            OleDbCommand command = new OleDbCommand(sql, connection);
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string n = reader[1].ToString();
                double p = Convert.ToDouble(reader[3]);
                double pt = Convert.ToDouble(reader[4]);
                int a = Convert.ToInt32(reader[5]);
                list.Add(new Product(n, p, pt, a));
            }
        }
        static void FillAbesDataFromDb(List<Product> abesItem)
        {
            OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;
                                      Data Source=C:\Users\HomeUser\Desktop\wpf\Rolling\Rolling\RollingDB.mdb");
            connection.Open();
            string sql = "SELECT * FROM Stock WHERE Nomenclature = 'Абестоцементные изделия'";
            OleDbCommand command = new OleDbCommand(sql, connection);
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string n = reader[1].ToString();
                double p = Convert.ToDouble(reader[3]);
                double pt = Convert.ToDouble(reader[4]);
                int a = Convert.ToInt32(reader[5]);
                abesItem.Add(new Product(n, p, pt, a));
            }
        }
        static void FillCatodDataFromDb(List<Product> catodAnod)
        {
            OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;
                                      Data Source=C:\Users\HomeUser\Desktop\wpf\Rolling\Rolling\RollingDB.mdb");
            connection.Open();
            string sql = "SELECT * FROM Stock WHERE Nomenclature = 'Катоды и аноды'";
            OleDbCommand command = new OleDbCommand(sql, connection);
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string n = reader[1].ToString();
                double p = Convert.ToDouble(reader[3]);
                double pt = Convert.ToDouble(reader[4]);
                int a = Convert.ToInt32(reader[5]);
                catodAnod.Add(new Product(n, p, pt, a));
            }
        }
        static void FillTubesDataFromDb(List<Product> pipe)
        {
            OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;
                                      Data Source=C:\Users\HomeUser\Desktop\wpf\Rolling\Rolling\RollingDB.mdb");
            connection.Open();
            string sql = "SELECT * FROM Stock WHERE Nomenclature = 'Труба'";
            OleDbCommand command = new OleDbCommand(sql, connection);
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string n = reader[1].ToString();
                double p = Convert.ToDouble(reader[3]);
                double pt = Convert.ToDouble(reader[4]);
                int a = Convert.ToInt32(reader[5]);
                pipe.Add(new Product(n, p, pt, a));
            }
        }
        static void FillWiresDataFromDb(List<Product> wires)
        {
            OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;
                                      Data Source=C:\Users\HomeUser\Desktop\wpf\Rolling\Rolling\RollingDB.mdb");
            connection.Open();
            string sql = "SELECT * FROM Stock WHERE Nomenclature = 'Проволока'";
            OleDbCommand command = new OleDbCommand(sql, connection);
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string n = reader[1].ToString();
                double p = Convert.ToDouble(reader[3]);
                double pt = Convert.ToDouble(reader[4]);
                int a = Convert.ToInt32(reader[5]);
                wires.Add(new Product(n, p, pt, a));
            }
        }
        #endregion

    }
}
