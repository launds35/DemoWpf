using DemoWpf.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace DemoWpf
{
    /// <summary>
    /// Логика взаимодействия для ItemUserControl.xaml
    /// </summary>
    public partial class ItemUserControl : UserControl
    {

        private Good GoodExemplar { get; set; }
        public event Action Edited;
        private bool _isEditing = false;
        private void RefreshItem()
        {
            title.Content = GoodExemplar.Category + " | " + GoodExemplar.Label;
            description.Content = "Описание товара: " + GoodExemplar.Desctiption;
            fabric.Content = "Производитель: " + GoodExemplar.Fabric;
            provider.Content = "Поставщик: " + GoodExemplar.Provider;

            if (GoodExemplar.Discount > 0)
            {
                decimal newPrice = (decimal)GoodExemplar.Price - ((decimal)GoodExemplar.Price * ((decimal)GoodExemplar.Discount / 100));
                price.Text = GoodExemplar.Price.ToString();
                price.TextDecorations = TextDecorations.Strikethrough;
                price.Foreground = Brushes.Red;
                priceDiscount.Text = newPrice.ToString();

            }
            else
            {
                price.Text = GoodExemplar.Price.ToString();
                priceDiscount.Text = "";
            }

            unit_of_measure.Content = "Единица измерения: " + GoodExemplar.Unit_of_measure;
            count.Content = "Количество на складе: " + GoodExemplar.Count.ToString();
            discount.Content = GoodExemplar.Discount.ToString() + "%";

            if (GoodExemplar.Discount > 15)
            {
                discountBorder.Background = (Brush)Application.Current.Resources["BigDiscountBrush"];
            }

            if (!(GoodExemplar.Photo is null))
            {
                photo.Source = new BitmapImage(new Uri($"pack://application:,,,/pictures/{GoodExemplar.Photo}", UriKind.Absolute));
            }

        }

        public ItemUserControl(Good good, string role)
        {
            InitializeComponent();
            GoodExemplar = good;

            if (role == "Администратор")
            {
                ItemFragment.MouseDoubleClick += Edit_Click;
            }

            RefreshItem();
        }

        

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (_isEditing)
            {
                return;
            }
            _isEditing = true;
            CrudGoodsWindow editWindow = new CrudGoodsWindow(GoodExemplar)
            {
                Owner = Application.Current.MainWindow
            };
            editWindow.Closed += (s, args) =>
            {
                Edited?.Invoke();
                _isEditing = false;
            };
            editWindow.ShowDialog();
        }
    }
}
