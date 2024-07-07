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
using System.Timers;
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
    public partial class changeStaff : Window, INotifyPropertyChanged
    {
        private ObservableCollection<TypeStaff> typeStaff;
        public ObservableCollection<TypeStaff> TypeStaff
        {
            get { return typeStaff; }
            set
            {
                typeStaff = value;
                OnPropertyChanged();
            }
        }
        public static int[] shifts = { 1, 2 };

        private Staff _staff;

        public Staff _Staff
        {
            get { return _staff; }
            set
            {
                _staff = value;
                OnPropertyChanged();
            }
        }
        RestaurantEntities context = new RestaurantEntities();

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public changeStaff(Staff sf, RestaurantEntities sharedContext)
        {
            InitializeComponent();
            context = sharedContext;
            _Staff = sf;
            TypeStaff = new ObservableCollection<TypeStaff>(context.TypeStaffs.ToList());
            DataContext = this;
        }


        private void btnSumbit_Click(object sender, RoutedEventArgs e)
        {
            if (_Staff.Person == null || string.IsNullOrWhiteSpace(_Staff.Person.PersonName) ||
                string.IsNullOrWhiteSpace(_Staff.Person.Telefon) || cbType.SelectedItem == null || cbShift.SelectedItem == null)
            {
                MessageBox.Show("Molimo popunite sva polja");
            }
            else if (_Staff.Person.PersonName.Any(c => char.IsDigit(c)))
            {
                MessageBox.Show("Ime ne sme sadrzati brojeve.");
            }
            else if (context.People.Any(p => p.Telefon == _Staff.Person.Telefon && p.PersonName != _Staff.Person.PersonName))
            {
                MessageBox.Show($"Uneti broj telefona već postoji.");
            }
            else if (_Staff.Person.Telefon.Any(char.IsLetter))
            {
                MessageBox.Show("Broj telefona ne sme sadržati slova.");
            }
            else
            {
               
                _Staff.Person.PersonName = _Staff.Person.PersonName;
                _Staff.Person.Telefon = _Staff.Person.Telefon;
                _Staff.shiftStaff = _Staff.shiftStaff;
                _Staff.TypeStaff1 = _Staff.TypeStaff1;
                context.SaveChanges();
                this.Close();
            }
        }




        

        private void ReloadStaffFromDatabase()
        {
            context.Entry(_Staff).Reload();
        }

        private void btnFold_Click(object sender, RoutedEventArgs e)
        {
            ReloadStaffFromDatabase();
            this.Close();
        }
        private void cbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbType.SelectedItem != null)
            {
                var selectedType = cbType.SelectedItem as TypeStaff;

                if (selectedType != null && _Staff != null)
                {
                    var staffToUpdate = context.Staffs.FirstOrDefault(c => c.idStaff == _Staff.idStaff);
                    if (staffToUpdate != null)
                    {
                        if (context.Entry(selectedType).State == System.Data.Entity.EntityState.Detached)
                        {
                            context.TypeStaffs.Attach(selectedType);
                        }

                        staffToUpdate.TypeStaff1 = selectedType;
                        context.SaveChanges();
                    }
                }
            }
        }


        private void cbShift_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbShift.SelectedItem != null)
            {
                var selectedShift = (int)cbShift.SelectedItem;
                _Staff.shiftStaff = selectedShift;
                context.SaveChanges();
            }
        }


    }
}
