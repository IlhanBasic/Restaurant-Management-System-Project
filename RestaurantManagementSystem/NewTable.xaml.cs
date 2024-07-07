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
    public partial class NewTable : Window,INotifyPropertyChanged
    {
        RestaurantEntities context = new RestaurantEntities();
        private C_Table _table;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public C_Table _Table
        {
            get { return _table; }
            set 
            { 
                _table = value;
                OnPropertyChanged();
            }
        }

        public NewTable()
        {
            InitializeComponent();
            _Table = new C_Table();
            _Table.NameTab = string.Empty;
        }

        private void btnSumbit_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(_Table.NameTab))
            {
                MessageBox.Show("Morate uneti ime");
            }
            
            else
            {
                
                context.C_Table.Add(_Table);
                context.SaveChanges();
                MessageBox.Show("Uspesno ste dodali sto");
                this.Close();
            }
        }

        private void btnFold_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
