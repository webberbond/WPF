using System;
using System.Collections.Generic;
using System.Data;
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
using System.Data.SqlClient;

namespace supply
{
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();
            LoadGrid();
        }

        private readonly SqlConnection sqlConnection =
            new SqlConnection(@"Data Source=WEBBER\SQLEXPRESS;Initial Catalog=supplier;Integrated Security=True");

        public void LoadGrid()
        {
            SqlCommand loadGrid = new SqlCommand("SELECT * FROM supply_db", sqlConnection);
            DataTable dataTable = new DataTable();
            sqlConnection.Open();
            SqlDataReader dataReader = loadGrid.ExecuteReader();
            dataTable.Load(dataReader);
            sqlConnection.Close();
            nameData.ItemsSource = dataTable.DefaultView;
        }

        public void clearData()
        {
            Type.Clear();
            Amount.Clear();
            Supply.Clear();
            Price.Clear();
            IDtxt.Clear();
        }

        public bool isValid()
        {
            if (Type.Text == string.Empty)
            {
                MessageBox.Show("Type is required");
                return false;
            }
            if (Amount.Text == string.Empty)
            {
                MessageBox.Show("Amount is required");
                return false;
            }
            if (Supply.Text == string.Empty)
            {
                MessageBox.Show("Supply is required");
                return false;
            }
            if (Price.Text == string.Empty)
            {
                MessageBox.Show("Price is required");
                return false;
            }

            return true;
        }


        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            sqlConnection.Open();
            SqlCommand editData = new SqlCommand("UPDATE supply_db SET type_of = '" + Type.Text + "', count_of = '" +
                                                    Amount.Text + "', supply = '" + Supply.Text + "', price = '" +
                                                    Price.Text + "' WHERE ID = '"+IDtxt.Text+"'", sqlConnection);
            try
            {
                editData.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
                LoadGrid();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            sqlConnection.Open();
            SqlCommand deleteCommand = new SqlCommand("DELETE FROM supply_db WHERE id = '" + IDtxt.Text+"'", sqlConnection);
            try
            {
                deleteCommand.ExecuteNonQuery();
                MessageBox.Show("SUCCESS");
                sqlConnection.Close();
                clearData();
                LoadGrid();
                sqlConnection.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("ERROR" +ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isValid())
                {
                    SqlCommand insertData = new SqlCommand(
                        "INSERT INTO supply_db VALUES(@type_of, @count_of, @supply, @price)",
                        sqlConnection);
                    insertData.CommandType = CommandType.Text;
                    insertData.Parameters.AddWithValue("@type_of", Type.Text);
                    insertData.Parameters.AddWithValue("@count_of", Amount.Text);
                    insertData.Parameters.AddWithValue("@supply", Supply.Text);
                    insertData.Parameters.AddWithValue("@price", Price.Text);
                    sqlConnection.Open();
                    insertData.ExecuteNonQuery();
                    sqlConnection.Close();
                    LoadGrid();
                    clearData();
                }
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
