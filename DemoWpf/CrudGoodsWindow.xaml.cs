using DemoWpf.Data;
using DemoWpf.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;


namespace DemoWpf
{
    /// <summary>
    /// Логика взаимодействия для CrudGoodsWindow.xaml
    /// </summary>
    public partial class CrudGoodsWindow : Window
    {
        private Good CurrentGood { get; set; }
        private string PhotoName = null;
        public CrudGoodsWindow(Good good)
        {
            InitializeComponent();
            CurrentGood = good;
            if (CurrentGood != null)
            {
                PhotoName = CurrentGood.Photo;
            } else
            {
                PhotoName = null;
            }
                FileLabel.Content = $"Фото ({PhotoName})";

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
                        Photo = PhotoName
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
                        newGood.Discount,
                        newGood.Photo
                    );

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
                        newGood.Discount,
                        newGood.Photo
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

        private void FileButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Выберите изображения";
                openFileDialog.Filter = "Изображения (*.jpg;*.png)|*.jpg;*.png";
                openFileDialog.Multiselect = false;


                if (openFileDialog.ShowDialog() != true)
                    return;

                string sourcePath = openFileDialog.FileName;

                string pictureDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pictures");
                Directory.CreateDirectory(pictureDir);


                string fileName = Path.GetFileName(sourcePath);
                string targetPath = Path.Combine(pictureDir, fileName);

                ResizeAndSaveImage(sourcePath, targetPath, 300, 200);
                FileLabel.Content = $"Фото ({fileName})";
                PhotoName = fileName;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
            
        }
        private void ResizeAndSaveImage(string sourcePath, string targetPath, int width, int height)
        {
            BitmapImage bitmap = new BitmapImage();
            
            using (FileStream fs = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
            {
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = fs;
                bitmap.DecodePixelWidth = width;
                bitmap.DecodePixelHeight = height;
                bitmap.EndInit();
            }

            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            using (FileStream fs = new FileStream(targetPath, FileMode.Create, FileAccess.Write))
            {
                encoder.Save(fs);
            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Вы уверены, что хотите удалить этот товар?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );
            
            if(result == MessageBoxResult.Yes)
            {
                DbHelpers.DeleteGood(CurrentGood.Article);
                Article.Text = "";
                Description.Text = "";
                Price.Text = "";
                Count.Text = "";
                Discount.Text = "";
                UnitOfMeasure.Text = "";
                MessageBox.Show("Товар удален!");
            }
        }
    }
}
