using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace NeedleWorkShop2
{
    public partial class AccountForm : Form
    {
        Database db = new Database(); //подключение к бд
        public AccountForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen; //расположение по центру экрана
            bool hasAccount = Presence();  //активация метода проверки наличия аккаунта у пользователя
        }
        public static int ID_Customer { get; set; }
        private Boolean Presence() //метод, проверяющий наличие резюме у пользователя
        {
            int userId = AuthorizationForm.ID_User;
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            DataTable table = new DataTable();
            string query = $"select ID_Customer, Last_Name, First_Name, Patronymic,Phone_Number, Address," +
                $" ID_User from Customer where ID_User='{userId}'"; //выбока данных определенного пользователя
            MySqlCommand command = new MySqlCommand(query, db.GetConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);
            if (table.Rows.Count > 0) //проверка наличия данных о пользователе
            {
                db.OpenConnection();
                MySqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read()) //вывод данных
                {
                    LastNameTextBox.Text = dataReader[1].ToString();
                    FirstNameTextBox.Text = dataReader[2].ToString();
                    PatronymicTextBox.Text = dataReader[3].ToString();
                    PhoneNumberTextBox.Text = dataReader[4].ToString();
                    AddressTextBox.Text = dataReader[5].ToString();
                }
                db.CloseConnection();
                return true;
            }
            else
            {
                return false;
            }
        }
        private Boolean Check() //метод, проверяющий чтобы все обязательные поля были заполнены
        {
            var lastName = LastNameTextBox.Text;
            var firstName = FirstNameTextBox.Text;
            var patronymic = PatronymicTextBox.Text;
            var phoneNumber = PhoneNumberTextBox.Text;
            var address = AddressTextBox.Text;
            if (string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(patronymic)
                || string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(address))
            {
                MessageBox.Show("Вы заполнили не все обязательные поля!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            else
            {
                return false;
            }
        }
        private void CustomerButton_Click(object sender, EventArgs e)
        {
            var lastName = LastNameTextBox.Text;
            var firstName = FirstNameTextBox.Text;
            var patronymic = PatronymicTextBox.Text;
            var phoneNumber = PhoneNumberTextBox.Text;
            var address = AddressTextBox.Text;
            int userId = AuthorizationForm.ID_User;
            string query = $"update Customer set Last_Name = '{lastName}', First_Name = '{firstName}', " +
                $"Patronymic = '{patronymic}', Phone_Number = '{phoneNumber}', Address = '{address}' " +
                $"where ID_User = '{userId}'"; //данные о покупателе в базе данных
            MySqlCommand command = new MySqlCommand(query, db.GetConnection());
            db.OpenConnection();
            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Данные сохранены!", "Успех!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            db.CloseConnection();
        }
        private void OrderButton_Click(object sender, EventArgs e)
        {
            int userId = AuthorizationForm.ID_User;
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            DataTable table = new DataTable();
            string query = $"select * from Customer where ID_User ='{userId}'"; //выборка определенного покупателя
            MySqlCommand command = new MySqlCommand(query, db.GetConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);
            if (table.Rows.Count == 1)
            {
                db.OpenConnection();
                MySqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    AccountForm.ID_Customer = (int)dataReader["ID_Customer"];
                }
                db.CloseConnection();
            }
            OrderForm orderButton = new OrderForm();
            this.Close();
            orderButton.Show(); //переход к форме заказов
        }
        private void ExitButton_Click(object sender, EventArgs e)
        {
            CustomerMainForm customerMainForm = new CustomerMainForm();
            this.Close();
            customerMainForm.Show(); //переход к главной форме
        }
    }
}