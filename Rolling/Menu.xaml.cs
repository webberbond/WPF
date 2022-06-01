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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rolling
{
    public partial class Menu : Window
    {
        readonly string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                  Data Source=C:\Users\HomeUser\Desktop\wpf\Rolling\Rolling\RollingDB.mdb";

        private readonly OleDbConnection myConnection;
        public static Menu Main { get; set; }
        public Menu()
        {
            InitializeComponent();
            myConnection = new OleDbConnection(connectionString);
            myConnection.Open();
        }

        public void WindowClose()
        {
            Close();
        }

        #region События для элементов меню
        private void OpenCatalog(object sender, RoutedEventArgs e)
        {
            Catalog catalog = new Catalog();
            catalog.Show();
        }
        private void CloseApplication(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void OpenRegistrationWindow(object sender, RoutedEventArgs e)
        {
            Login autorise = new Login();
            autorise.Show();
        }

        private void AboutProgram_Click(object sender, RoutedEventArgs e)
        {
            AboutBox about = new AboutBox();
            about.Show();
        }
        private void Contacts_Click(object sender, RoutedEventArgs e)
        {
            NewMessageBox nmb = new NewMessageBox();
            nmb.Show();
            nmb.ErrorText.Text = "Hotline: +8(800)555-35-35 \n\nE-Mail: exampleadress@gmail.com";
        }
#pragma warning disable S4144 // Methods should not have identical implementations
        private void Button_Click(object sender, RoutedEventArgs e)
#pragma warning restore S4144 // Methods should not have identical implementations
        {
            Catalog catalog = new Catalog();
            catalog.Show();
        }

        #region Очистка данных

        private void ClearAll(object sender, RoutedEventArgs e)
        {
            ComboItemsBox.SelectedIndex = 0;
            TxtAmount.Text = "0";
            TxtFio.Text = "Full Name";
            TxtMail.Text = "Email - Optional";
            TxtPhone.Text = "Phone Number";
            TxtDir.Text = "Address";
            RadioSelfdelivery.IsChecked = true;
            LbInfo.Content = $"Total Price: 0 rubles.";
            LbInfo2.Content = "";
        }
        private void ClearItemInfo(object sender, RoutedEventArgs e)
        {
            ComboItemsBox.SelectedIndex = 0;
            TxtAmount.Text = "0";
        }
        private void ClearPersonalInfo(object sender, RoutedEventArgs e)
        {
            TxtFio.Text = "Full Name";
            TxtMail.Text = "Email - Optional";
            TxtPhone.Text = "Phone Number";
            TxtDir.Text = "Address";
            RadioSelfdelivery.IsChecked = true;
        }
        #endregion

        #endregion
        private void TxtAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            PriceAccumulation();
        }

        #region Обработка событий по нажатию клавиш клавиатуры
        //private void TxtAmount_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Space)
        //    {
        //        e.Handled = false; // если пробел, отклоняем ввод
        //    }
        //}

        //private void TxtAmount_TextInput(object sender, TextCompositionEventArgs e)
        //{
        //    int val;
        //    if (!Int32.TryParse(e.Text, out val) && e.Text != "-")
        //    {
        //        e.Handled = true; // отклоняем ввод
        //    }
        //}
        #endregion

        private void PriceAccumulation()
        {
            try
            {
                double price = Convert.ToDouble(TxtAmount.Text) * Convert.ToDouble(TxtPrice.Text);
                if (RadioMail.IsChecked == true)
                {
                    price += 50;
                }
                else if (RadioCourier.IsChecked == true)
                {
                    price += 1200;
                }
                else
                {
                    price = Convert.ToDouble(TxtAmount.Text) * Convert.ToDouble(TxtPrice.Text);
                }
                LbInfo.Content = $"Total Price:";
                LbInfo2.Content = $"{price}";
            }
            catch
            {
                LbInfo.Content = "Price calculation error";
                LbInfo2.Content = "";
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            PriceAccumulation();
            NewMessageBox nmb = new NewMessageBox();
            if (TxtFio.Text == "" || TxtPhone.Text == "" || TxtDir.Text == "" || TxtAmount.Text == "0")
            {
                nmb.Show();

                nmb.ErrorText.Text = "One or more fields are empty.";
            }
            else
            {
                try
                {
                    InsertOrder();
                    UpdateProduct();
                    TxtFio.Text = "Full Name";
                    TxtMail.Text = "Email - Optional";
                    TxtPhone.Text = "Phone Number";
                    TxtDir.Text = "Address";
                    TxtName.Text = "Name";
                    TxtPrice.Text = "Price";
                    TxtSize.Text = "Price per ton";
                    TxtWeight.Text = "Stock remainder";
                    TxtAmount.Text = "0";
                    LbInfo.Content = "Total price:";
                    nmb.Show();
                    nmb.ErrorText.Text = "Your order has been successfully placed, please wait for response. \nClick OK to return to the checkout menu.";
                }
                catch
                {
                    nmb.Show();
                    nmb.ErrorText.Text = "The input data was not in the correct format";
                }
            }
        }
        private void InsertOrder()
        {
            string client = TxtFio.Text;
            string email = TxtMail.Text;
            string phone = TxtPhone.Text;
            string adress = TxtDir.Text;
            string product = TxtName.Text;
            int amount = Convert.ToInt32(TxtAmount.Text);
            string delivery = DeliveryCheck();
            double price = Convert.ToDouble(LbInfo2.Content);
            DateTime date = DateTime.Now;
            string query = "INSERT INTO Orders (Client, [Email], [Number], Address, Goods, Amount, [Delivery Type], [Total Price], [Order Time]) VALUES ('" + client + "', '" + email + "', '" + phone + "', '" + adress + "', '" + product + "', '" + amount + "', '" + delivery + "', '" + price + "', '" + date + "')";
            OleDbCommand command = new OleDbCommand(query, myConnection);
            command.ExecuteNonQuery();
        }

        private string DeliveryCheck()
        {
            string delivery;
            if (RadioMail.IsChecked == true)
            {
                delivery = "Mail delivery";
                return delivery;
            }
            else if (RadioCourier.IsChecked == true)
            {
                delivery = "Courier delivery";
                return delivery;
            }
            else
            {
                delivery = "Pickup";
                return delivery;
            }
        }

        private void UpdateProduct()
        {
            string name = TxtName.Text;
            int amountLeft = Convert.ToInt32(TxtWeight.Text);
            int amount = Convert.ToInt32(TxtAmount);
            int newAmountLeft = amountLeft - amount;
            string query = "UPDATE Stock SET Remainder = '" + newAmountLeft + "' WHERE Name = '" + name + "'";
            OleDbCommand command = new OleDbCommand(query, myConnection);
            command.ExecuteNonQuery();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT * FROM Stock";
            OleDbCommand command = new OleDbCommand(sql, myConnection);
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string n = reader[1].ToString();
                ComboItemsBox.Items.Add(n);
            }
        }

        private void ComboItemsBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string itemName = ComboItemsBox.SelectedItem.ToString();
            string sql = "SELECT * FROM Stock WHERE Name = '" + itemName + "'";
            OleDbCommand command = new OleDbCommand(sql, myConnection);
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                TxtName.Text = reader[1].ToString();
                TxtPrice.Text = reader[3].ToString();
                TxtSize.Text = reader[4].ToString();
                TxtWeight.Text = reader[5].ToString();
                AmountSlider.Maximum = Convert.ToDouble(TxtWeight.Text);
            }
        }

        private void TxtFIO_MouseEnter(object sender, MouseEventArgs e)
        {
            if (TxtFio.Text == "Full Name")
            {
                TxtFio.Text = "";
            }
        }

        private void TxtMail_MouseEnter(object sender, MouseEventArgs e)
        {
            if (TxtMail.Text == "Email - Optional")
            {
                TxtMail.Text = "";
            }
        }

        private void TxtPhone_MouseEnter(object sender, MouseEventArgs e)
        {
            if (TxtPhone.Text == "Phone Number")
            {
                TxtPhone.Text = "";
            }
        }

        private void TxtDir_MouseEnter(object sender, MouseEventArgs e)
        {
            if (TxtDir.Text == "Address")
            {
                TxtDir.Text = "";
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NewMessageBox nmb = new NewMessageBox();
            switch (OtherComboBox.SelectedIndex)
            {
                case 0:
                    nmb.Show();
                    nmb.ErrorText.Text = "We are working on it.";
                    break;
                case 1:
                    nmb.Show();
                    nmb.ErrorText.Text = "We are adding it.";
                    break;
                default:
                    break;
            }
        }
    }
}
