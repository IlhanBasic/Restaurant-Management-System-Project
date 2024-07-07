using Microsoft.Win32;
using RestaurantManagementSystem.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RestaurantManagementSystem
{
    /// <summary>
    /// Interaction logic for changeProduct.xaml
    /// </summary>
    public partial class changeProduct : Window, INotifyPropertyChanged
    {
        public ObservableCollection<Category> productCategory { get; set; } = new ObservableCollection<Category>();
        private Article _product;
       
        public Article Product
        {
            get { return _product; }
            set
            {
                _product = value;
                OnPropertyChanged();
            }
        }

        RestaurantEntities context;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public changeProduct(Article product,RestaurantEntities SharedContext)
        {
            context = new RestaurantEntities();
            InitializeComponent();
            Product = product;
            
            context = SharedContext;
            var temp = context.Categories.ToList();
            foreach (var c in temp)
            {
                productCategory.Add(c);
            }

            LoadImageFromDatabase();
            cbCateg.Items.Refresh();

            DataContext = this;
        }

        private void SaveImageToDatabase(string filePath, int articleId)
        {
            byte[] imageData = File.ReadAllBytes(filePath);

            var article = context.Articles.FirstOrDefault(a => a.idArticle == articleId);
            if (article != null)
            {
                article.imageArticle = imageData;
                context.SaveChanges();
            }
        }


        private void btnSumbit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Product.nameArticle) || Product.priceArticle.ToString().Any(char.IsLetter) ||
                Product.quantityArticle.ToString().Any(char.IsLetter)  || 
                Product.priceArticle<=0 || Product.quantityArticle<=0 || Product.Category==null)
            {
                MessageBox.Show("Molimo unesite ispravne vrednosti");
            }
            else
            {
                var productToUpdate = context.Articles.FirstOrDefault(c => c.idArticle == Product.idArticle);
                if (productToUpdate != null)
                {
                    context.SaveChanges();
                    LoadImageFromDatabase();
                }

                this.Close();
            }
            
        }

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                BitmapImage bitmap = new BitmapImage(new Uri(filePath));
                SelectedImage.Source = bitmap;

                SaveImageToDatabase(filePath, Product.idArticle);
            }
        }
        

        private void LoadImageFromDatabase()
        {
            if (Product.imageArticle != null)
            {
                using (var ms = new MemoryStream(Product.imageArticle))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = ms;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    SelectedImage.Source = bitmap;
                }
            }
        }

        private void btnFold_Click(object sender, RoutedEventArgs e)
        {
            ReloadProductFromDatabase();
            this.Close();
        }
        private void ReloadProductFromDatabase()
        {
            context.Entry(Product).Reload();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton != null)
            {
                if (radioButton.Content.ToString() == "DA")
                {
                    Product.activityArticle = true;
                }
                else if (radioButton.Content.ToString() == "NE")
                {
                    Product.activityArticle = false;
                }
                OnPropertyChanged(nameof(Product.activityArticle));
            }
        }

        private void cbCateg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbCateg.SelectedItem != null)
            {
                var selectedCategory = cbCateg.SelectedItem as Category;

                if (selectedCategory != null)
                {
                    Product.categoryID = selectedCategory.IdCat;

                    context.SaveChanges();
                }
            }
        }
    }
}
