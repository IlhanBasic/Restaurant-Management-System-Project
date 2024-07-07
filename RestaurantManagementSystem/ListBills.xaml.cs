using RestaurantManagementSystem.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Windows.Shapes;

namespace RestaurantManagementSystem
{
    /// <summary>
    /// Interaction logic for ListBills.xaml
    /// </summary>
    public partial class ListBills : Window,INotifyPropertyChanged
    {
        public Bill selectedBill;
        public bool flag = false;
        RestaurantEntities context = new RestaurantEntities();
        private ObservableCollection<Bill> bills;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ObservableCollection<Bill> Bills
        {
            get { return bills; }
            set { 
                bills = value;
                OnPropertyChanged();
            }
        }

        public ListBills()
        {
            Bills = new ObservableCollection<Bill>();
            var temp = context.Bills.ToList ();
            foreach(var item in temp)
            {
                Bills.Add (item);
            }
            
            InitializeComponent();
           
        }
        private void RefreshList()
        {
            Bills = new ObservableCollection<Bill>(context.Bills.ToList());
        }
        private bool _isFirstEdit = true;
        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

            if (_isFirstEdit)
            {
                _isFirstEdit = false;
                ((DataGrid)sender).IsReadOnly = true; 
            }
        }

        private void WriteBillToFile(Bill bill, string typePayment, float discount = 0, float paid = 0)
        {
            string filePath = "bills.txt"; 
            FileStream fileStream = null;
            StreamWriter writer = null;

            try
            {
                fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write);
                writer = new StreamWriter(fileStream);

                writer.WriteLine($"------------Račun ID: {bill.Id}----------");
                writer.WriteLine($"Rezervacija ID: {bill.IdReservation}");
                writer.WriteLine($"Sto: {bill.Reservation.C_Table.NameTab}");
                writer.WriteLine($"Konobar: {bill.Reservation.Staff.Person.PersonName}");
                writer.WriteLine($"Tip plaćanja: {typePayment}");
                writer.WriteLine($"Ukupan iznos: {bill.BillItems.Sum(p => p.TotalPrice)}");

                if (typePayment == "keš")
                {
                    float totalAmount =(float) bill.BillItems.Sum(p => p.TotalPrice);
                    float discountedAmount = totalAmount - (totalAmount * discount / 100);
                    float change = paid - discountedAmount;
                    
                    writer.WriteLine($"Popust: {discount}%");
                    writer.WriteLine($"Uplaćeno: {paid}");
                    writer.WriteLine($"Kusur: {change}");
                    writer.WriteLine($"Neto iznos: {discountedAmount}");
                }

                writer.WriteLine($"Datum: {bill.Reservation.Datum}\n");
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
        }

        private void btnCard_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgListBIlls.SelectedItem as Bill;
            if (selected != null)
            {
                if (selected.StatusBill == "gotov")
                {


                    MessageBox.Show("Račun je plaćen karticom.");

                    selected.StatusBill = "placen";

                    var reservations = context.Reservations.ToList();
                    var reservationForRemove = reservations.FirstOrDefault(r => r.Id == selected.IdReservation);
                    var billItems = context.BillItems.Where(item => item.BillId == selected.Id).ToList();
                    WriteBillToFile(selected, "kartica");
                    context.BillItems.RemoveRange(billItems);
                    context.Bills.Remove(selected);
                    context.Reservations.Remove(reservationForRemove);
                    context.SaveChanges();
                    RefreshList();
                }
                else
                {
                    MessageBox.Show("Narudžbina mora biti gotova da bi se naplatila");
                }
            }
            else
            {
                MessageBox.Show("Morate prvo izabrati račun koju želite da naplatite.");
            }
        }

        private void btnCache_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgListBIlls.SelectedItem as Bill;
            if (selected != null)
            {
                if (selected.StatusBill == "gotov")
                {
                    paymentDetails pd = new paymentDetails(selected);
                    pd.Closed += (s, args) =>
                    {
                        if (pd.paymentSuccessful)
                        {
                            
                            MessageBox.Show("Račun je plaćen gotovinom.");
                            selected.StatusBill = "placen";
                            try
                            {
                                var reservations = context.Reservations.ToList();
                                WriteBillToFile(selected, "keš",pd.Popust,pd.Uplaceno);
                                var reservationForRemove = reservations.FirstOrDefault(r => r.Id == selected.IdReservation);
                                if (reservationForRemove != null)
                                {
                                    context.Reservations.Remove(reservationForRemove);
                                }
                                var billItems = context.BillItems.Where(item => item.BillId == selected.Id).ToList();
                                context.BillItems.RemoveRange(billItems);
                                context.Bills.Remove(selected);
                                context.SaveChanges();
                                RefreshList();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Došlo je do greške prilikom brisanja računa i rezervacije: " + ex.Message);
                            }
                        }
                    };
                    pd.Show();
                }
                else
                {
                    MessageBox.Show("Narudžbina mora biti gotova da bi se naplatila");
                }
            }
            else
            {
                MessageBox.Show("Morate prvo izabrati račun koji želite da naplatite.");
            }
        }


    }
}
