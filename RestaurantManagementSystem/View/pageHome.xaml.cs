using System;
using System.Collections.Generic;
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
    /// Interaction logic for pageHome.xaml
    /// </summary>
    public partial class pageHome : Page, INotifyPropertyChanged
    {
        RestaurantEntities context = new RestaurantEntities();
        private Restaurant _restaurant;

        public Restaurant Restaurant
        {
            get { return _restaurant; }
            set
            {
                _restaurant = value;
                OnPropertyChanged();
            }
        }

        public pageHome()
        {
            InitializeComponent();
                Restaurant = context.Restaurants.FirstOrDefault();
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
