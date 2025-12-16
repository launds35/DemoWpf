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
        private string Role {  get; set; }

        public MainWindow(string role, string fullName)
        {
            InitializeComponent();
            Role = role;
            fioLabel.Content = fullName;

            Goods = DbHelpers.GetGoodsList();

            goodsView = CollectionViewSource.GetDefaultView(Goods);
            goodsView.Filter = FilterGoods;

            RenderItems();
            InitRoleComponents(Role);
        }

        private void LoadGoods()
        {
            ItemsPanel.Children.Clear();

            Goods = DbHelpers.GetGoodsList();
            foreach (Good good in Goods)
            {
                var item = new ItemUserControl(good, Role);
                item.Edited += LoadGoods;
                ItemsPanel.Children.Add(item);
            }
        }

        private bool FilterGoods(object obj)
        {
            if(!(obj is Good good))
                return false;

            string search = SearchBox.Text?.ToLower();

            bool searchMatch = string.IsNullOrWhiteSpace(search) ||
                good.Category?.ToLower().Contains(search) == true ||
                good.Label?.ToLower().Contains(search) == true ||
                good.Provider?.ToLower().Contains(search) == true ||
                good.Article?.ToLower().Contains(search) == true ||
                good.Fabric?.ToLower().Contains(search) == true ||
                good.Desctiption?.ToLower().Contains(search) == true;

            bool providerMatch = true;

            if (FilterBox.SelectedItem is ComboBoxItems selectedProvider)
            {
                if (selectedProvider.Id == 0)
                {
                    providerMatch = true;
                }
                else
                {
                    providerMatch = good.Provider == selectedProvider.Name;
                }
            }
                         
            return searchMatch && providerMatch;
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
                var item = new ItemUserControl(good, Role);
                item.Edited += LoadGoods;
                ItemsPanel.Children.Add(item);
            }
        }

        private void InitFilterBox()
        {
            List<ComboBoxItems> providers = DbHelpers.GetProviders();
            providers.Insert(0, new ComboBoxItems { Id = 0, Name = "Все поставщики" });
            FilterBox.ItemsSource = providers;
            FilterBox.DisplayMemberPath = "Name";
            FilterBox.SelectedValuePath = "Id";
            FilterBox.SelectedIndex = 0;
        }

        private void InitRoleComponents(string role)
        {
            if (role == null)
            {
                this.Title = "Главное окно (Гость)";
                fioLabel.Visibility = Visibility.Collapsed;
                backButton.Content = "Назад";

                Add.Visibility = Visibility.Collapsed;
                SortLabel.Visibility = Visibility.Collapsed;    
                FilterLabel.Visibility = Visibility.Collapsed;    
                SearchLabel.Visibility = Visibility.Collapsed;
                SearchBox.Visibility = Visibility.Collapsed;
                FilterBox.Visibility = Visibility.Collapsed;
                SortBox.Visibility = Visibility.Collapsed;
            }
            else if (role == "Авторизированный клиент")
            {
                this.Title = "Главное окно (Авторизированный клиент)";

                Add.Visibility = Visibility.Collapsed;
                SortLabel.Visibility = Visibility.Collapsed;
                FilterLabel.Visibility = Visibility.Collapsed;
                SearchLabel.Visibility = Visibility.Collapsed;
                SearchBox.Visibility = Visibility.Collapsed;
                FilterBox.Visibility = Visibility.Collapsed;
                SortBox.Visibility = Visibility.Collapsed;
            }
            else if (role == "Менеджер")
            {
                this.Title = "Главное окно (Менеджер)";
                Add.Visibility = Visibility.Collapsed;
                InitFilterBox();
            }
            else if (role == "Администратор")
            {
                this.Title = "Главное окно (Администратор)";
                InitFilterBox();
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            CrudGoodsWindow addWindow = new CrudGoodsWindow(null);
            addWindow.Closed += (s, args) => LoadGoods();
            addWindow.Show();
        }

        private void SortBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (goodsView == null)
                return;

            goodsView.SortDescriptions.Clear();

            switch (SortBox.SelectedIndex)
            {
                case 1:
                    goodsView.SortDescriptions.Add(
                        new SortDescription(nameof(Good.Count), ListSortDirection.Ascending)    
                    );
                    break;
                case 2:
                    goodsView.SortDescriptions.Add(
                        new SortDescription(nameof(Good.Count), ListSortDirection.Descending)
                    );
                    break;
            }

            goodsView.Refresh();
            RenderItems();
        }

        private void FilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (goodsView == null) return;

            goodsView.Refresh();
            RenderItems();
        }
    }
}
