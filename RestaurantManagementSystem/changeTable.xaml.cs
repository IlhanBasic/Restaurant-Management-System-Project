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
using System.Windows.Shapes;

namespace RestaurantManagementSystem
{
    public partial class changeTable : Window,INotifyPropertyChanged
    {
        private C_Table _tables;

        public C_Table _Tables
        {
            get { return _tables; }
            set 
            { 
                _tables = value;
                OnPropertyChanged();
            }
        }

        RestaurantEntities context = new RestaurantEntities();

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public changeTable(C_Table category)
        {
            InitializeComponent();
            _Tables = category;
            tbIme.Text = _Tables.NameTab;
        }

        private void btnSumbit_Click(object sender, RoutedEventArgs e)
        {
            string temp = _Tables.NameTab;
            _Tables.NameTab = tbIme.Text;
            if (!string.IsNullOrEmpty(_Tables.NameTab))
            {
                var categoryToUpdate = context.C_Table.FirstOrDefault(c => c.IdTab == _tables.IdTab);
                if (categoryToUpdate != null)
                {
                    categoryToUpdate.NameTab = _tables.NameTab;
                    context.SaveChanges();
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Morate dati ime stolu");
                _Tables.NameTab = temp;
            }
                
        }


        private void btnFold_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
