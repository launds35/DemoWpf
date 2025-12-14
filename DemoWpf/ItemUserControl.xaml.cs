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
        public ItemUserControl(Good good)
        {
            InitializeComponent();
            title.Content = good.category + " | " + good.label;
            description.Content = good.desctiption;
            fabric.Content = good.fabric;
            provider.Content = good.provider;
            price.Content = good.price.ToString();
            unit_of_measure.Content = good.unit_of_measure;
            count.Content = good.count.ToString();
            discount.Content = good.discount.ToString() + "%";
            if (good.discount > 15)
            {
                discountBorder.Background = (Brush)Application.Current.Resources["BigDiscountBrush"];
            }
            if(!(good.photo is null))
            {
                photo.Source = new BitmapImage(new Uri($"pack://application:,,,/pictures/{good.photo}", UriKind.Absolute));
            }
        }
    }
}
