using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;

namespace RestaurantManagementSystem
{
    public partial class changeCategory : Window, INotifyPropertyChanged
    {
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

        RestaurantEntities context = new RestaurantEntities();

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public changeCategory(Category category)
        {
            InitializeComponent();
            _Category = category;
            tbIme.Text = _Category.NameCat;
        }

        private void btnSumbit_Click(object sender, RoutedEventArgs e)
        {
            string before = _Category.NameCat;
            _Category.NameCat = tbIme.Text.Trim();

            if (string.IsNullOrEmpty(_Category.NameCat) || _Category.NameCat.Any(char.IsDigit))
            {
                MessageBox.Show("Morate uneti ispravno ime");
                _Category.NameCat = before;
            }
            else
            {
                var categoryToUpdate = context.Categories.FirstOrDefault(c => c.IdCat == _category.IdCat);
                if (categoryToUpdate != null)
                {
                    categoryToUpdate.NameCat = _Category.NameCat;
                    context.SaveChanges();
                }
                this.Close();
            }
        }

        private void btnFold_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
