using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace RestaurantManagementSystem
{
    

    public partial class NewStaff : Window, INotifyPropertyChanged
    {
        RestaurantEntities context = new RestaurantEntities();
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<TypeStaff> typeStaff { get; set; } = new ObservableCollection<TypeStaff>();
        public static int[] shifts = { 1, 2 };
        private Staff newStaff;
        private Person person;
        public Person Person
        {
            get { return person; }
            set
            {
                person = value;
                OnPropertyChanged();
            }
        }

        public Staff _NewStaff
        {
            get { return newStaff; }
            set
            {
                newStaff = value;
                OnPropertyChanged();
            }
        }

        public NewStaff()
        {
            var temp = context.TypeStaffs.ToList();
            foreach (var c in temp)
            {
                typeStaff.Add(c);
            }
            InitializeComponent();
            var restaurant = context.Restaurants.FirstOrDefault();
            person = new Person
            {
                PersonName = "",
                Telefon = "",
                idRestaurant = restaurant.RestaurantID
            };
            _NewStaff = new Staff
            {
                PersonID = person.PersonID,
                shiftStaff = 0,
                TypeStaff1 = typeStaff.FirstOrDefault(),
                Person = Person 
            };
            DataContext = this;
        }

        private void btnSumbit_Click(object sender, RoutedEventArgs e)
        {
            bool isExistingTelephone = context.People.Any(p => p.Telefon == _NewStaff.Person.Telefon);

            if (_NewStaff.Person == null || string.IsNullOrWhiteSpace(_NewStaff.Person?.PersonName) || _NewStaff.shiftStaff == 0 ||
                string.IsNullOrWhiteSpace(_NewStaff.Person.Telefon) || _NewStaff.TypeStaff1 == null)
            {
                MessageBox.Show("Molimo popunite sva polja");
            }
            else if (_NewStaff.Person.PersonName.Any(c => char.IsDigit(c)))
            {
                MessageBox.Show("Ime ne sme sadrzati brojeve.");
            }

            else if (isExistingTelephone)
            {
                MessageBox.Show($"Uneti broj telefona već postoji.");
            }
            else if (_NewStaff.Person.Telefon.Any(char.IsLetter))
            {
                MessageBox.Show("Broj telefona ne sme sadržati slova.");
            }

            else
            {
                context.People.Add(Person);
                context.SaveChanges();
                context.Staffs.Add(_NewStaff);
                context.SaveChanges();

                this.Close();
            }
        }

        

        private void btnFold_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }

}
