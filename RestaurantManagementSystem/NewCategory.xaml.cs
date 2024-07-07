using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace RestaurantManagementSystem
{
    /// <summary>
    /// Interaction logic for NewCategory.xaml
    /// </summary>
    public partial class NewCategory : Window,INotifyPropertyChanged
    {
        RestaurantEntities context = new RestaurantEntities();

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private Category _category;

        public Category _Category
        {
            get { return _category; }
            set 
            { 
                _category = value;
                OnPropertyChanged();
            }
        }

        public NewCategory()
        {
            InitializeComponent();
            _Category = new Category();
            _Category.NameCat=string.Empty;
        }

        private void btnSumbit_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(_Category.NameCat) || !_Category.NameCat.All(char.IsLetter))
            {
                MessageBox.Show("Morate uneti ispravno ime");
            }
            else
            {
                context.Categories.Add(_Category);
                context.SaveChanges();
                MessageBox.Show("Uspesno ste dodali kategoriju");
                this.Close();
            }
        }

        private void btnFold_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
