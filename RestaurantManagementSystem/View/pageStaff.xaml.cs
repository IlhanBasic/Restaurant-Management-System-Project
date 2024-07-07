using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for pageStaff.xaml
    /// </summary>
    public partial class pageStaff : Page
    {
        public pageStaff()
        {
            
            InitializeComponent();
        }

        private void btnStaff_Click(object sender, RoutedEventArgs e)
        {
            pageStaffShow pss = new pageStaffShow();
            showPage.Content = pss;
            btnStaff.Background = Brushes.Red;
            btnTypeStaff.Background = (Brush)this.FindResource("bgColor");
        }

        private void btnTypeStaff_Click(object sender, RoutedEventArgs e)
        {
            pageStaffType pss = new pageStaffType();
            showPage.Content = pss;
            btnTypeStaff.Background = Brushes.Red;
            btnStaff.Background = (Brush)this.FindResource("bgColor");
        }
    }
}
