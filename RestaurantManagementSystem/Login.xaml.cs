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
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window,INotifyPropertyChanged
    {
        RestaurantEntities context = new RestaurantEntities();
        private User _user;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        ObservableCollection<User> Users { get; set; }
        public User _User
        {
            get { return _user; }
            set 
            { 
                _user = value;
                OnPropertyChanged();
            }
        }

        public Login()
        {
            InitializeComponent();
            Users = new ObservableCollection<User>();
            _User = new User();
            var temp = context.Users.ToList();
            foreach (var user in temp)
            {
                Users.Add(user);
            }
            
        }
        
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var username = tbUsername.Text;
            var password = tbPassword.Password;

            _User.username = username;
            _User.uPass = password;

            var existedUser = context.Users.Any(p => p.uPass == password && p.username == username);
            if (existedUser)
            {
                this.Hide();
                MainWindow mw = new MainWindow(_User);
                mw.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Pogresno korisnicko ime ili šifra");
            }
        }




    }
}
