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

namespace DemoWpf
{
    /// <summary>
    /// Логика взаимодействия для ItemUserControl.xaml
    /// </summary>
    public partial class ItemUserControl : UserControl
    {

        private Good GoodExemplar { get; set; }
        public ItemUserControl(Good good, string role)
        {
            InitializeComponent();
            GoodExemplar = good;

            if (role != "Администратор")
            {
                Edit.Visibility = Visibility.Collapsed;
            }

            title.Content = good.Category + " | " + good.Label;
            description.Content = "Описание товара: " + good.Desctiption;
            fabric.Content = "Производитель: " + good.Fabric;
            provider.Content = "Поставщик: " + good.Provider;

            if (good.Discount > 0)
            {
                decimal newPrice = (decimal)good.Price - ((decimal)good.Price * ((decimal)good.Discount / 100));
                price.Text = good.Price.ToString();
                price.TextDecorations = TextDecorations.Strikethrough;
                price.Foreground = Brushes.Red;
                priceDiscount.Text = newPrice.ToString();

            } 
            else
            {
                price.Text = good.Price.ToString();
                priceDiscount.Text = "";
            }

            unit_of_measure.Content = "Единица измерения: " + good.Unit_of_measure;
            count.Content = "Количество на складе: " + good.Count.ToString();
            discount.Content = good.Discount.ToString() + "%";

            if (good.Discount > 15)
            {
                discountBorder.Background = (Brush)Application.Current.Resources["BigDiscountBrush"];
            }

            if(!(good.Photo is null))
            {
                photo.Source = new BitmapImage(new Uri($"pack://application:,,,/pictures/{good.Photo}", UriKind.Absolute));
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            CrudGoodsWindow editWindow = new CrudGoodsWindow(GoodExemplar);
            editWindow.Show();
        }
    }
}
