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
using System.Windows.Shapes;

namespace DemoWpf
{
    /// <summary>
    /// Логика взаимодействия для CrudGoodsWindow.xaml
    /// </summary>
    public partial class CrudGoodsWindow : Window
    {
        public CrudGoodsWindow(Good good)
        {
            InitializeComponent();
            if (good != null)
            {
                this.Title = "Редактирование товара";

                Article.Text = good.Article;
                Category.Text = good.Category;
                Label.Text = good.Label;
                Description.Text = good.Desctiption;
                Fabric.Text = good.Fabric;
                Provider.Text = good.Provider;
                Price.Text = good.Price.ToString();
                UnitOfMeasure.Text = good.Unit_of_measure;
                Count.Text = good.Count.ToString();
                Discount.Text = good.Discount.ToString();

            }
        }
    }
}
