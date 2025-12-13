using DemoWpf.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DemoWpf.Data;

namespace DemoWpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Good> Goods { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Goods = DbHelpers.GetGoodsList();
            foreach (var good in Goods)
            {
                try
                {
                    var itemControl = new ItemUserControl(good);
                    ItemsPanel.Children.Add(itemControl);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении товара {good.article ?? "null"}: {ex.Message}");
                }
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
