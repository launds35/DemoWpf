using DemoWpf.Data;
using DemoWpf.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace DemoWpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Good> Goods { get; set; }
        private ICollectionView goodsView;

        public MainWindow(string role, string full_name)
        {
            InitializeComponent();
            fioLabel.Content = full_name;
            InitRoleComponents(role);

            Goods = DbHelpers.GetGoodsList();

            goodsView = CollectionViewSource.GetDefaultView(Goods);
            goodsView.Filter = FilterGoods;
            RenderItems();
        }

        private bool FilterGoods(object obj)
        {
            if(!(obj is Good good))
                return false;

            string search = SearchBox.Text?.ToLower();

            if (string.IsNullOrWhiteSpace(search))
                return true;

            return good.category?.ToLower().Contains(search) == true ||
                good.label?.ToLower().Contains(search) == true ||
                good.provider?.ToLower().Contains(search) == true ||
                good.article?.ToLower().Contains(search) == true ||
                good.fabric?.ToLower().Contains(search) == true ||
                good.desctiption?.ToLower().Contains(search) == true;

        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            goodsView.Refresh();
            RenderItems();
        }

        private void RenderItems()
        {
            ItemsPanel.Children.Clear();

            foreach (Good good in goodsView)
            {
                ItemsPanel.Children.Add(new ItemUserControl(good));
            }
        }

        private void InitRoleComponents(string role)
        {
            if (role == null)
            {
                SearchBox.Visibility = Visibility.Hidden;
                this.Title = "Главное окно (Гость)";
                fioLabel.Visibility = Visibility.Hidden;
                backButton.Content = "Назад";
            }
            else if (role == "Администратор")
            {
                this.Title = "Главное окно (Администратор)";

            }
            else if (role == "Менеджер") 
            {
                this.Title = "Главное окно (Менеджер)";
            } 
            else if (role == "Авторизированный клиент")
            {
                this.Title = "Главное окно (Авторизированный клиент)";

            }
            
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
