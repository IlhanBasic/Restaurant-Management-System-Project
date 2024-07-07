using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace RestaurantManagementSystem.View
{
    public partial class pageStaffShow : Page, INotifyPropertyChanged
    {
        private RestaurantEntities context = new RestaurantEntities();

        private ObservableCollection<Staff> staffs;
        public ObservableCollection<Staff> Staffs
        {
            get { return staffs; }
            set
            {
                staffs = value;
                OnPropertyChanged();
            }
        }

        public pageStaffShow()
        {
            InitializeComponent();
            LoadData();
            DataContext = this;
        }

        private void LoadData()
        {
            Staffs = new ObservableCollection<Staff>(context.Staffs.Include("TypeStaff1").Include("Person").ToList());
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                var filteredStaffs = context.Staffs.Include("TypeStaff1").Include("Person")
                    .Where(staff => staff.Person.PersonName.ToLower().StartsWith(searchTerm.ToLower()))
                    .ToList();

                if (filteredStaffs.Any())
                {
                    Staffs = new ObservableCollection<Staff>(filteredStaffs);
                }
                else
                {
                    MessageBox.Show("Nema rezultata za uneti pojam.", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
                    RefreshStaffs();
                }
            }
            else
            {
                RefreshStaffs();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            NewStaff np = new NewStaff();
            np.Closed += (s, args) =>
            {
                RefreshStaffs();
            };
            np.Show();
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgProducts.SelectedItem as Staff;
            if (selected != null)
            {
                changeStaff cc = new changeStaff(selected, context); // prosledite context
                cc.Closed += (s, args) => RefreshStaffs();
                cc.Show();
            }
            else
            {
                MessageBox.Show("Morate prvo izabrati zaposlenog kojeg želite da promenite.");
            }
        }


        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgProducts.SelectedItem as Staff;
            if (selected != null)
            {
                MessageBoxResult result = MessageBox.Show("Da li ste sigurni da želite da obrišete zaposlenog?", "Potvrda brisanja", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    var staffToRemove = context.Staffs.FirstOrDefault(p => p.idStaff == selected.idStaff);
                    var osoba = context.People.FirstOrDefault(p => p.PersonID == staffToRemove.PersonID);
                    if (staffToRemove != null && osoba!=null)
                    {
                        context.People.Remove(osoba);
                        context.Staffs.Remove(staffToRemove);
                        context.SaveChanges();
                        RefreshStaffs();
                    }
                }
                else
                {
                    MessageBox.Show("Zaposleni nije obrisan.", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Morate prvo izabrati zaposlenog kojeg želite da obrišete.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private bool _isFirstEdit = true;
        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (_isFirstEdit)
            {
                _isFirstEdit = false;
                ((DataGrid)sender).IsReadOnly = true; 
            }
        }

        private void RefreshStaffs()
        {
            Staffs = new ObservableCollection<Staff>(context.Staffs.Include("TypeStaff1").Include("Person").ToList());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
