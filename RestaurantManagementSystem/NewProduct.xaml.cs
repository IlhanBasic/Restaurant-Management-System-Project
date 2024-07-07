using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace RestaurantManagementSystem
{
    
    public partial class NewProduct : Window,INotifyPropertyChanged
    {
        RestaurantEntities context = new RestaurantEntities();
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private Article newArticle;

        public Article NewArticle
        {
            get { return newArticle; }
            set 
            { 
                newArticle = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Category> newCategories { get; set; }= new ObservableCollection<Category>();

        public NewProduct()
        {
            InitializeComponent();
            var categoriesList = context.Categories.ToList();

            foreach (var category in categoriesList)
            {
                newCategories.Add(category);
            }

            var initCategory = categoriesList.FirstOrDefault();
            var restaurant = context.Restaurants.FirstOrDefault();
            NewArticle = new Article
            {
                idRestaurant=restaurant.RestaurantID,
                nameArticle = "",
                priceArticle = 0,
                quantityArticle = 0,
                unitArticle = "",
                categoryID = initCategory?.IdCat ?? 0, 
                Category = initCategory,
                imageArticle = File.ReadAllBytes("Home.png")
            };

            DataContext = this; 
        }

        private void btnSumbit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NewArticle.nameArticle) || NewArticle.priceArticle == 0 ||
                NewArticle.Category == null || string.IsNullOrWhiteSpace(NewArticle.Category.NameCat) ||
                NewArticle.quantityArticle == 0 || string.IsNullOrWhiteSpace(NewArticle.unitArticle) ||
                NewArticle.imageArticle == null || NewArticle.imageArticle.Length == 0)
            {
                MessageBox.Show("Molimo popunite sva polja");
                if (radBtnYes.IsChecked != true && radBtnNo.IsChecked != true)
                {
                    radBtnNo.IsChecked = true;
                }
            }
            else if (!NewArticle.priceArticle.ToString().All(char.IsDigit) || 
                !NewArticle.quantityArticle.ToString().All(char.IsDigit))
            {
                MessageBox.Show("Molimo unesite ispravne vrednosti ");
            }
            else
            {
                context.Articles.Add(NewArticle);
                context.SaveChanges();

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

                SaveImageToDatabase(filePath, NewArticle.idArticle);
            }
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
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton != null)
            {
                if (radioButton.Content.ToString() == "DA")
                {
                    NewArticle.activityArticle = true;
                }
                else if (radioButton.Content.ToString() == "NE")
                {
                    NewArticle.activityArticle = false;
                }
            }
        }

        private void btnFold_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
