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
    public partial class pageStaffType : Page, INotifyPropertyChanged
    {
        RestaurantEntities context = new RestaurantEntities();
        private ObservableCollection<TypeStaff> typeStaffs;

        public ObservableCollection<TypeStaff> TypeStaffs
        {
            get { return typeStaffs; }
            set
            {
                typeStaffs = value;
                OnPropertyChanged();
            }
        }


        public pageStaffType()
        {

            TypeStaffs = new ObservableCollection<TypeStaff>();
            var temp = context.TypeStaffs.ToList();
            foreach (var c in temp)
            {
                TypeStaffs.Add(c);
            }
            InitializeComponent();

            
        }


        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            // Pretraga
            if (!string.IsNullOrEmpty(searchTerm))
            {
                var filteredCategories = TypeStaffs.Where(cat => cat.nameType.ToLower().StartsWith(searchTerm.ToLower())).ToList();
                if (filteredCategories.Count > 0)
                {
                    TypeStaffs.Clear();
                    foreach (var v in filteredCategories)
                    {
                        TypeStaffs.Add(v);
                    }
                }
                else
                {
                    MessageBox.Show("Nema rezultata za uneti pojam.", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                TypeStaffs.Clear();
                var temp = context.TypeStaffs.ToList();
                foreach (var c in temp)
                {
                    TypeStaffs.Add(c);
                }
            }
        }


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            NewStaffType nw = new NewStaffType();
            nw.Closed += (s, args) => RefreshTypeStaff();
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
            var selected = dgType.SelectedItem as TypeStaff;
            if (selected != null)
            {
                changeTypeStaff cc = new changeTypeStaff(selected);
                cc.Closed += (s, args) => RefreshTypeStaff();
                cc.Show();
            }
            else
            {
                MessageBox.Show("Morate prvo izabrati tip koju želite da promenite.");
            }
        }

        private void RefreshTypeStaff()
        {
            TypeStaffs = new ObservableCollection<TypeStaff>(context.TypeStaffs.ToList());
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgType.SelectedItem as TypeStaff;
            if (selected != null)
            {
                MessageBoxResult result = MessageBox.Show("Da li ste sigurni da zelite da obrisete tip zaposlenog?", "Potvrda brisanja", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    var typeToRemove = context.TypeStaffs.FirstOrDefault(c => c.idType == selected.idType);
                    if (typeToRemove != null)
                    {
                        context.TypeStaffs.Remove(typeToRemove);
                        context.SaveChanges();
                        RefreshTypeStaff();
                    }
                }
                else
                {
                    MessageBox.Show("tip nije obrisan.", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Morate prvo izabrati tip koju želite da obrišete.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


    }
}
