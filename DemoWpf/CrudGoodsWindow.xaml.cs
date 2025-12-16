using DemoWpf.Data;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DemoWpf
{
    /// <summary>
    /// Логика взаимодействия для CrudGoodsWindow.xaml
    /// </summary>
    public partial class CrudGoodsWindow : Window
    {
        private Good CurrentGood { get; set; }
        public CrudGoodsWindow(Good good)
        {
            InitializeComponent();
            CurrentGood = good;
            List<ComboBoxItems> categories = DbHelpers.GetCategories();
            List<ComboBoxItems> fabrics = DbHelpers.GetFabrics();
            List<ComboBoxItems> labels = DbHelpers.GetLabels();
            List<ComboBoxItems> providers = DbHelpers.GetProviders();

            Category.ItemsSource = categories;
            Category.DisplayMemberPath = "Name";
            Category.SelectedValuePath = "Id";

            Fabric.ItemsSource = fabrics;
            Fabric.DisplayMemberPath = "Name";
            Fabric.SelectedValuePath = "Id";

            Label.ItemsSource = labels;
            Label.DisplayMemberPath = "Name";
            Label.SelectedValuePath = "Id";

            Provider.ItemsSource = providers;
            Provider.DisplayMemberPath = "Name";
            Provider.SelectedValuePath = "Id";
            
            //Проверка на то откуда открыто окно, из поля редактировать или добавить
            if (good != null)
            {
                this.Title = "Редактирование товара";

                for (int i = 0; i < categories.Count; i++) 
                {
                    if (categories[i].Name == good.Category)
                    {
                        Category.SelectedIndex = i;
                    }
                }

                for (int i = 0; i < fabrics.Count; i++)
                {
                    if (fabrics[i].Name == good.Fabric)
                    {
                        Fabric.SelectedIndex = i;
                    }
                }

                for (int i = 0; i < labels.Count; i++)
                {
                    if (labels[i].Name == good.Label)
                    {
                        Label.SelectedIndex = i;
                    }
                }

                for (int i = 0; i < providers.Count; i++)
                {
                    if (providers[i].Name == good.Provider)
                    {
                        Provider.SelectedIndex = i;
                    }
                }

                Article.Text = good.Article;
                Description.Text = good.Desctiption;
                Price.Text = good.Price.ToString();
                Count.Text = good.Count.ToString();
                Discount.Text = good.Discount.ToString();
                UnitOfMeasure.Text = good.Unit_of_measure;
                AddButton.Content = "Редактировать";
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Category.SelectedItem == null ||
                    Fabric.SelectedItem == null ||
                    Label.SelectedItem == null ||
                    Provider.SelectedItem == null)
                {
                    MessageBox.Show("Заполните все выпадающие списки");
                    return;
                }
                var category = (ComboBoxItems)Category.SelectedItem;
                var fabric = (ComboBoxItems)Fabric.SelectedItem;
                var label = (ComboBoxItems)Label.SelectedItem;
                var provider = (ComboBoxItems)Provider.SelectedItem;

                string _photo = null;
                if (!(CurrentGood is null))
                {
                    _photo = CurrentGood.Photo;
                }
                

                Good newGood = new Good
                    {
                        Article = Article.Text,
                        Category = category.Name,
                        Desctiption = Description.Text,
                        Price = Convert.ToDouble(Price.Text),
                        Count = Convert.ToInt32(Count.Text),
                        Discount = Convert.ToInt32(Discount.Text),
                        Unit_of_measure = UnitOfMeasure.Text,
                        Fabric = fabric.Name,
                        Label = label.Name,
                        Provider = provider.Name,
                        Photo = _photo
                    };

                if (AddButton.Content.ToString() == "Редактировать")
                {
                
                    bool result = DbHelpers.UpdateGood(
                        CurrentGood.Article,
                        newGood.Article,
                        category.Id,
                        label.Id,
                        newGood.Desctiption,
                        fabric.Id,
                        provider.Id,
                        newGood.Price,
                        newGood.Unit_of_measure,
                        newGood.Count,
                        newGood.Discount);

                    if (result)
                    {
                        MessageBox.Show("Успех");
                    }
                }
                else
                {
                    bool result = DbHelpers.AddGood(
                        newGood.Article,
                        category.Id,
                        label.Id,
                        newGood.Desctiption,
                        fabric.Id,
                        provider.Id,
                        newGood.Price,
                        newGood.Unit_of_measure,
                        newGood.Count,
                        newGood.Discount
                    );

                    if (result)
                    {
                        MessageBox.Show("Успех");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
