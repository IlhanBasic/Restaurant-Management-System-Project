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
    /// Interaction logic for pageTables.xaml
    /// </summary>
    public partial class pageTables : Page, INotifyPropertyChanged
    {
        RestaurantEntities context = new RestaurantEntities();
        private ObservableCollection<C_Table> tables;

        public ObservableCollection<C_Table> Tables
        {
            get { return tables; }
            set
            {
                tables = value;
                OnPropertyChanged();
            }
        }


        public pageTables()
        {

            Tables = new ObservableCollection<C_Table>();
            var temp = context.C_Table.ToList();
            foreach (var c in temp)
            {
                Tables.Add(c);
            }
            InitializeComponent();

            
        }


        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            // Pretraga
            if (!string.IsNullOrEmpty(searchTerm))
            {
                var filteredTables = Tables.Where(cat => cat.NameTab.ToLower().StartsWith(searchTerm.ToLower())).ToList();

                if (filteredTables.Count > 0)
                {
                    Tables.Clear();
                    foreach (var v in filteredTables)
                    {
                        Tables.Add(v);
                    }
                }
                else
                {
                    MessageBox.Show("Nema rezultata za uneti pojam.", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                Tables.Clear();
                var temp = context.C_Table.ToList();
                foreach (var c in temp)
                {
                    Tables.Add(c);
                }
            }
        }


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            NewTable nw = new NewTable();
            nw.Closed += (s, args) => RefreshTables();
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
            var selected = dgCat.SelectedItem as C_Table;
            if (selected != null)
            {
                changeTable cc = new changeTable(selected);
                cc.Closed += (s, args) => RefreshTables();
                cc.Show();
            }
            else
            {
                MessageBox.Show("Morate prvo izabrati sto koju želite da promenite.");
            }
        }

        private void RefreshTables()
        {
            Tables = new ObservableCollection<C_Table>(context.C_Table.ToList());
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgCat.SelectedItem as C_Table;
            if (selected != null)
            {
                MessageBoxResult result = MessageBox.Show("Da li ste sigurni da zelite da obrisete sto?", "Potvrda brisanja", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    var tableToRemove = context.C_Table.FirstOrDefault(c => c.IdTab == selected.IdTab);
                    if (tableToRemove != null)
                    {
                        context.C_Table.Remove(tableToRemove);
                        context.SaveChanges();
                        RefreshTables();
                    }
                }
                else
                {
                    MessageBox.Show("Sto nije obrisan.", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Morate prvo izabrati sto koju želite da obrišete.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


    }
}
