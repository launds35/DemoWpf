using DemoWpf.Data;
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

namespace DemoWpf
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Window
    {
        public Auth()
        {
            InitializeComponent();
        }

        private void guestButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow(null, "Гость");
            main.Closed += (s, args) => this.Show();
            main.Show();
            this.Hide();

        }

        private void enterButton_Click(object sender, RoutedEventArgs e)
        {
            string login = loginBox.Text;
            string password = passwordBox.Password;
            passwordBox.Clear();

            var user = DbHelpers.Authorize(login, password);

            password = null;

            if(user != null)
            {
                MainWindow main = new MainWindow(user.Role, $"{user.Surname} {user.Name} {user.LastName}");
                main.Closed += (s, args) => this.Show();
                main.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Введен неверный логин или пароль!", "Ошибка авторизации");
            }
        }
    }
}
