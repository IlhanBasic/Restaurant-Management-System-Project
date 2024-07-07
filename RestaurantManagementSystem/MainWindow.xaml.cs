using RestaurantManagementSystem.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RestaurantManagementSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string korisnik = "";
        public MainWindow(User user)
        {
            InitializeComponent();
            tbUser.Text = "Username: " + user.username;
            korisnik = user.username;
            tbOption.Text = "Home";
            tbDate.Text = "Datum: " + DateTime.Now.ToString("dd.MM.yyyy");
            pageHome p = new pageHome();
            showPage.Content = p;
        }
       
        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            tbOption.Text = "Home";

            if (showPage.Content != null)
            {
                ((Page)showPage.Content).NavigationService.RemoveBackEntry();
            }

            pageHome p = new pageHome();
            showPage.Content = p;
        }


        private void btnCategories_Click(object sender, RoutedEventArgs e)
        {
            if (korisnik == "admin")
            {
                tbOption.Text = "Kategorije";

                if (showPage.Content != null)
                {
                    ((Page)showPage.Content).NavigationService.RemoveBackEntry();
                }

                pageCategories p = new pageCategories();
                showPage.Content = p;
            }
            else
            {
                MessageBox.Show($"Morate biti admin da biste pristupili");
            }

        }

        private void btnProducts_Click(object sender, RoutedEventArgs e)
        {
            if (korisnik == "admin")
            {
                tbOption.Text = "Proizvodi";

                if (showPage.Content != null)
                {
                    ((Page)showPage.Content).NavigationService.RemoveBackEntry();
                }

                pageProducts p = new pageProducts();
                showPage.Content = p;
            }
            else
            {
                MessageBox.Show($"Morate biti admin da biste pristupili");
            }
            
        }

        private void btnStaff_Click(object sender, RoutedEventArgs e)
        {
            if (korisnik == "admin")
            {
                tbOption.Text = "Osoblje";

                if (showPage.Content != null)
                {
                    ((Page)showPage.Content).NavigationService.RemoveBackEntry();
                }

                pageStaff p = new pageStaff();
                showPage.Content = p;
            }
            else
            {
                MessageBox.Show($"Morate biti admin da biste pristupili");
            }
            
        }

        private void btnPOS_Click(object sender, RoutedEventArgs e)
        {
            if (korisnik == "konobar" || korisnik == "admin")
            {
                tbOption.Text = "Placanja";

                if (showPage.Content != null)
                {
                    ((Page)showPage.Content).NavigationService.RemoveBackEntry();
                }

                pagePOS p = new pagePOS();
                showPage.Content = p;
            }
            else
            {
                MessageBox.Show($"Morate biti konobar ili admin da biste pristupili");
            }
            
        }

        private void btnKitchen_Click(object sender, RoutedEventArgs e)
        {
            if(korisnik =="admin" || korisnik == "kuvar")
            {
                tbOption.Text = "Kuhinja";

                if (showPage.Content != null)
                {
                    ((Page)showPage.Content).NavigationService.RemoveBackEntry();
                }
                pageKitchen p = new pageKitchen();
                showPage.Content = p;
            }
            else
            {
                MessageBox.Show($"Morate biti kuvar ili admin da biste pristupili");
            }
        }

        private void btnTables_Click(object sender, RoutedEventArgs e)
        {
            if (korisnik == "admin")
            {
                tbOption.Text = "Stolovi";

                if (showPage.Content != null)
                {
                    ((Page)showPage.Content).NavigationService.RemoveBackEntry();
                }

                pageTables p = new pageTables();
                showPage.Content = p;
            }
            else
            {
                MessageBox.Show($"Morate biti admin da biste pristupili");
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Da li ste sigurni da zelite da se izlogujete ?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                Login l = new Login();
                l.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Ostali ste ulogovani.", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            
        }
    }
}
