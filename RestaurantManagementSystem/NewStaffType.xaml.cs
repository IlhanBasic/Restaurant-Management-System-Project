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
    public partial class NewStaffType : Window,INotifyPropertyChanged
    {
        RestaurantEntities context = new RestaurantEntities();

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private TypeStaff _type;

        public TypeStaff _Type
        {
            get { return _type; }
            set 
            { 
                _type = value;
                OnPropertyChanged();
            }
        }

        public NewStaffType()
        {
            InitializeComponent();
            _Type = new TypeStaff();
        }

        private void btnSumbit_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(_Type.nameType) || _Type.nameType.Any(char.IsDigit))
            {
                MessageBox.Show("Morate uneti ispravno ime");
            }
            else
            {

                context.TypeStaffs.Add(_Type);
                context.SaveChanges();
                MessageBox.Show("Uspesno ste dodali tip zaposlenog");

                this.Close();
            }
        }

        private void btnFold_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
