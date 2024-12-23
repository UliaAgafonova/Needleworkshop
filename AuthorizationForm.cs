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
    public partial class AuthorizationForm : Form
    {
        Database db = new Database(); //подключение к бд
        public AuthorizationForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen; //расположение по центру экрана
        }
        private void AuthorizationForm_Load(object sender, EventArgs e)
        {
            PasswordTextBox.PasswordChar = '*'; //скрытие пароля символом
        }
        public static int ID_User { get; set; }
        private Boolean Check() //метод, проверяющий чтобы все обязательные поля были заполнены
        {
            var login = LoginTextBox.Text;
            var password = PasswordTextBox.Text;
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Вы заполнили не все обязательные поля!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            else
            {
                return false;
            }
        }
        private void AuthorizationButton_Click(object sender, EventArgs e)
        {
            if (Check()) //активация метода проверки пустых полей
            {
                return;
            }
            var login = LoginTextBox.Text;
            var password = PasswordTextBox.Text;
            var role = 0;
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            DataTable table = new DataTable();
            string query = $"select * from User where Login ='{login}' and Password ='{password}'"; //выборка данных из таблицы User
            MySqlCommand command = new MySqlCommand(query, db.GetConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);
            db.OpenConnection();
            MySqlDataReader RoleDataReader = command.ExecuteReader();
            while (RoleDataReader.Read()) //нахождение роли
            {
                role = (int)RoleDataReader["Role"];
            }
            db.CloseConnection();
            if (table.Rows.Count == 1) //проверка наличия такого пользователя
            {
                MessageBox.Show("Вы успешно вошли!", "Успех!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (role == 2)
                {
                    ManagerMainForm managerMainForm = new ManagerMainForm();
                    managerMainForm.Show(); //переход к главной форме для менеджера
                    this.Hide();
                }
                else
                {
                    db.OpenConnection();
                    MySqlDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read()) //вывод задач сотрудника
                    {
                        AuthorizationForm.ID_User = (int)dataReader["ID_User"];
                    }
                    db.CloseConnection();
                    CustomerMainForm customerMainForm = new CustomerMainForm();
                    customerMainForm.Show(); //переход к главной форме для сотрудника
                    this.Hide();
                }
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RegistrationLabel_Click(object sender, EventArgs e)
        {
            RegistrationForm registrationForm = new RegistrationForm();
            registrationForm.Show(); //переход к форме регистрации
            this.Hide();
        }
    }
}