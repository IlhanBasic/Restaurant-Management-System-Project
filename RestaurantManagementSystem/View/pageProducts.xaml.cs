using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace RestaurantManagementSystem.View
{
    /// <summary>
    /// Interaction logic for pageProducts.xaml
    /// </summary>
    public partial class pageProducts : Page, INotifyPropertyChanged
    {
        RestaurantEntities context = new RestaurantEntities();
        private Article product;

        public Article Product
        {
            get { return product; }
            set { product = value; }
        }

        private ObservableCollection<Article> articles;

        public ObservableCollection<Article> Articles
        {
            get { return articles; }
            set
            {
                articles = value;
                OnPropertyChanged();
            }
        }


        public pageProducts()
        {

            Articles = new ObservableCollection<Article>();
            var temp = context.Articles.ToList();
            foreach (var c in temp)
            {
                Articles.Add(c);
            }
            InitializeComponent();

            
        }


        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            // Pretraga
            if (!string.IsNullOrEmpty(searchTerm))
            {
                var filteredProducts = Articles.Where(pro => pro.nameArticle.ToLower().StartsWith(searchTerm.ToLower())).ToList();

                if (filteredProducts.Count > 0)
                {
                    Articles.Clear();
                    foreach (var v in filteredProducts)
                    {
                        Articles.Add(v);
                    }
                }
                else
                {
                    MessageBox.Show("Nema rezultata za uneti pojam.", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                Articles.Clear();
                var temp = context.Articles.ToList();
                foreach (var c in temp)
                {
                    Articles.Add(c);
                }
            }
        }


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            NewProduct np = new NewProduct();
            np.Closed += (s, args) => RefreshProducts(); 
            np.Show();
        }
        private bool _isFirstEdit = true;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

            if (_isFirstEdit)
            {
                _isFirstEdit = false;
                ((DataGrid)sender).IsReadOnly = true; 
            }
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgProducts.SelectedItem as Article;
            if (selected != null)
            {
                changeProduct cc = new changeProduct(selected,context);
                cc.Closed += (s, args) => RefreshProducts();
                cc.Show();
            }
            else
            {
                MessageBox.Show("Morate prvo izabrati kategoriju koju želite da promenite.");
            }
        }

        private void RefreshProducts()
        {
            Articles = new ObservableCollection<Article>(context.Articles.ToList());
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgProducts.SelectedItem as Article;
            if (selected != null)
            {
                MessageBoxResult result = MessageBox.Show("Da li ste sigurni da zelite da obrisete proizvod?", "Potvrda brisanja", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    var productToRemove = context.Articles.FirstOrDefault(p => p.idArticle == selected.idArticle);
                    if (productToRemove != null)
                    {
                        context.Articles.Remove(productToRemove);
                        context.SaveChanges();
                        RefreshProducts();
                    }
                }
                else
                {
                    MessageBox.Show("Proizvod nije obrisan.", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Morate prvo izabrati proizvod koji želite da obrišete.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    }

}
