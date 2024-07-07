using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for pageCategories.xaml
    /// </summary>
    public partial class pageCategories : Page,INotifyPropertyChanged
    {
        RestaurantEntities context = new RestaurantEntities();
        private ObservableCollection<Category> categories;

        public ObservableCollection<Category> Categories
        {
            get { return categories; }
            set 
            { 
                categories = value;
                OnPropertyChanged();
            }
        }


        public pageCategories()
        {
            
            Categories = new ObservableCollection<Category>();
            var temp = context.Categories.ToList();
            foreach (var c in temp)
            {
                Categories.Add(c);
            }
            InitializeComponent();

           
        }


        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            // Pretraga
            if (!string.IsNullOrEmpty(searchTerm))
            {
                var filteredCategories = Categories.Where(cat => cat.NameCat.ToLower().StartsWith(searchTerm.ToLower())).ToList();

                if (filteredCategories.Count > 0)
                {
                    Categories.Clear();
                    foreach(var v in filteredCategories)
                    {
                        Categories.Add(v);
                    }
                }
                else
                {
                    MessageBox.Show("Nema rezultata za uneti pojam.", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                Categories.Clear ();
                var temp = context.Categories.ToList();
                foreach (var c in temp)
                {
                    Categories.Add(c);
                }
            }
        }


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            NewCategory nw = new NewCategory();
            nw.Closed += (s, args) => RefreshCategories(); 
            nw.Show();
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
            var selected = dgCat.SelectedItem as Category;
            if (selected != null)
            {
                changeCategory cc = new changeCategory(selected);
                cc.Closed += (s, args) => RefreshCategories();
                cc.Show();
            }
            else
            {
                MessageBox.Show("Morate prvo izabrati kategoriju koju želite da promenite.");
            }
        }

        private void RefreshCategories()
        {
            Categories = new ObservableCollection<Category>(context.Categories.ToList());
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgCat.SelectedItem as Category;
            if (selected != null)
            {
                MessageBoxResult result = MessageBox.Show("Da li ste sigurni da zelite da obrisete kategoriju?", "Potvrda brisanja", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    var categoryToRemove = context.Categories.FirstOrDefault(c => c.IdCat == selected.IdCat);
                    if (categoryToRemove != null)
                    {
                        context.Categories.Remove(categoryToRemove);
                        context.SaveChanges();
                        RefreshCategories();
                    }
                }
                else
                {
                    MessageBox.Show("Kategorija nije obrisana.", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Morate prvo izabrati kategoriju koju želite da obrišete.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


    }
}
