using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace RestaurantManagementSystem
{
    /// <summary>
    /// Interaction logic for paymentDetails.xaml
    /// </summary>
   


    public partial class paymentDetails : Window, INotifyPropertyChanged
    {
        private float _brutoRacun;
        public float BrutoRacun
        {
            get { return _brutoRacun; }
            set
            {
                if (_brutoRacun != value)
                {
                    _brutoRacun = value;
                    OnPropertyChanged();
                    CalculateNetoRacun();
                }
            }
        }

        private float _popust;
        public float Popust
        {
            get { return _popust; }
            set
            {
                if (_popust != value)
                {
                    _popust = value;
                    OnPropertyChanged();
                    CalculateNetoRacun();
                }
            }
        }

        private float _uplaceno;
        public float Uplaceno
        {
            get { return _uplaceno; }
            set
            {
                if (_uplaceno != value)
                {
                    _uplaceno = value;
                    OnPropertyChanged();
                    CalculateKusur();
                }
            }
        }

        private string _netoRacun;
        public string NetoRacun
        {
            get { return _netoRacun; }
            set
            {
                if (_netoRacun != value)
                {
                    _netoRacun = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _kusur;
        public string Kusur
        {
            get { return _kusur; }
            set
            {
                if (_kusur != value)
                {
                    _kusur = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool paymentSuccessful = false;

        public paymentDetails(Bill bill)
        {
            InitializeComponent();
            BrutoRacun = (float)bill.BillItems.Sum(p => p.TotalPrice);
            Popust = 0;
            Uplaceno = 0;
            txtBrutoRacun.IsReadOnly = true;
            txtNetoRacun.IsReadOnly = true;
            txtKusur.IsReadOnly = true;
            CalculateNetoRacun();
        }

        private void btnSumbit_Click(object sender, RoutedEventArgs e)
        {
            if (float.TryParse(NetoRacun, out float netoRacunValue))
            {
                if (Uplaceno < netoRacunValue)
                {
                    MessageBox.Show("Iznos uplate mora biti veći od neto računa.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                paymentSuccessful = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Greška prilikom konverzije neto računa.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void btnFold_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtPopust_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CalculateNetoRacun();
        }

        private void txtUplaceno_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CalculateKusur();
        }

        private void CalculateNetoRacun()
        {
            if (Popust > 100)
            {
                MessageBox.Show("Popust ne sme biti veci od 100%");
            }
            else
            {
                float netoRacun = BrutoRacun - (BrutoRacun * (Popust / 100.0f));
                NetoRacun = netoRacun.ToString("0.00");
                CalculateKusur();
            }
            
        }

        private void CalculateKusur()
        {
            float kusur = Uplaceno - float.Parse(NetoRacun);
            Kusur = Math.Max(0, kusur).ToString("0.00"); 
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
