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
    public partial class changeTypeStaff : Window,INotifyPropertyChanged
    {
        private TypeStaff _typeStaff;

        public TypeStaff _TypeStaff
        {
            get { return _typeStaff; }
            set 
            { 
                _typeStaff = value;
                OnPropertyChanged();
            }
        }

        RestaurantEntities context = new RestaurantEntities();

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public changeTypeStaff(TypeStaff ts)
        {
            InitializeComponent();
            _TypeStaff = ts;
            tbIme.Text = _TypeStaff.nameType;
        }

        private void btnSumbit_Click(object sender, RoutedEventArgs e)
        {
            string before = _TypeStaff.nameType;
            _TypeStaff.nameType = tbIme.Text.Trim();
            if (string.IsNullOrEmpty(_TypeStaff.nameType) || _TypeStaff.nameType.Any(char.IsDigit))
            {
                MessageBox.Show("Molimo unesite ispravne podatke");
                _TypeStaff.nameType = before;
            }
            else
            {
                var typeToUpdate = context.TypeStaffs.FirstOrDefault(c => c.idType == _TypeStaff.idType);
                if (typeToUpdate != null)
                {
                    typeToUpdate.nameType = _TypeStaff.nameType;
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
