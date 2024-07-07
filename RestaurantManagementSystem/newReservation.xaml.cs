using RestaurantManagementSystem.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RestaurantManagementSystem
{
    /// <summary>
    /// Interaction logic for newReservation.xaml
    /// </summary>
    public partial class newReservation : Window
    {
        RestaurantEntities context = new RestaurantEntities();
        public ObservableCollection<Reservation> Reservations { get; set; } = new ObservableCollection<Reservation>();
        public ObservableCollection<BillItem> BillItems { get; set; } = new ObservableCollection<BillItem>();
        public ObservableCollection<C_Table> Tables { get; set; } = new ObservableCollection<C_Table>();
        public ObservableCollection<Staff> Waiters { get; set; } = new ObservableCollection<Staff>();
        private int tablesCols;
        private int tablesRows;
        private Button[,] tablesButtons;
        private bool isClickedTable = false;
        private bool isClickedWaiter = false;
        public C_Table chosenTable;
        public Staff chosenWaiter;
        private Button btnReserved;
        Bill newBill = new Bill();


        public bool IsReservationMade { get; private set; } = false;
        public int TablesCols
        {
            get { return tablesCols; }
            set
            {
                tablesCols = value;
                setTablesButtons();
            }
        }

        public int TablesRows
        {
            get { return tablesRows; }
            set
            {
                tablesRows = value;
                setTablesButtons();
            }
        }

        private int waitersCols;
        private int waitersRows;
        private Button[,] waitersButtons;

        public int WaitersCols
        {
            get { return waitersCols; }
            set
            {
                waitersCols = value;
                setWaitersButtons();
            }
        }

        public int WaitersRows
        {
            get { return waitersRows; }
            set
            {
                waitersRows = value;
                setWaitersButtons();
            }
        }

        public newReservation(ObservableCollection<BillItem> Items)
        {
            InitializeComponent();

            BillItems = Items;
            LoadTables();
            LoadWaiters();
            LoadReservations();
            TablesCols = Tables.Count;
            WaitersCols = Waiters.Count;
            
           
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            setTablesButtons();
            setWaitersButtons(); 
        }

        private void setTablesButtons()
        {
            layerTable.Children.Clear(); 
            tablesButtons = new Button[TablesRows, 2];
            int index = 0;
            for (int i = 0; i < TablesRows; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (index < Tables.Count)
                    {
                        tablesButtons[i, j] = new Button();
                        tablesButtons[i, j].Width = layerTable.ActualWidth / 2;
                        tablesButtons[i, j].Height = layerTable.ActualHeight / tablesRows;
                        tablesButtons[i, j].Background = (Brush)this.FindResource("bgColor");
                        tablesButtons[i, j].Content = $"{Tables[index].NameTab}";
                        tablesButtons[i, j].Click += TableButton_Click;
                        tablesButtons[i, j].Visibility = Visibility.Visible;
                        tablesButtons[i, j].Foreground = Brushes.White;
                        tablesButtons[i, j].Tag = Tables[index].IdTab;
                        int tableId = (int)tablesButtons[i, j].Tag;
                        bool hasReservations = Reservations.Any(p => p.IdTable == tableId);
                        if (hasReservations == true)
                        {
                            tablesButtons[i, j].Background = Brushes.Red;
                        }
                        Canvas.SetTop(tablesButtons[i, j], i * tablesButtons[i, j].Height);
                        Canvas.SetLeft(tablesButtons[i, j], j * tablesButtons[i, j].Width);
                        layerTable.Children.Add(tablesButtons[i, j]);
                        index++;
                    }
                }
            }
        }

        private void setWaitersButtons()
        {
            layerWaiter.Children.Clear();
            waitersButtons = new Button[WaitersRows, 2];
            int index = 0;
            for (int i = 0; i < WaitersRows; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (index < Waiters.Count)
                    {
                        waitersButtons[i, j] = new Button();
                        waitersButtons[i, j].Width = layerWaiter.ActualWidth / 2;
                        waitersButtons[i, j].Height = layerWaiter.ActualHeight / WaitersRows;
                        waitersButtons[i, j].Background = (Brush)this.FindResource("bgColor");
                        waitersButtons[i, j].Content = $"{Waiters[index].Person.PersonName}";
                        waitersButtons[i, j].Click += WaiterButton_Click;
                        waitersButtons[i, j].Visibility = Visibility.Visible;
                        waitersButtons[i, j].Foreground = Brushes.White;
                        waitersButtons[i, j].Tag = Waiters[index].idStaff;
                        Canvas.SetTop(waitersButtons[i, j], i * waitersButtons[i, j].Height);
                        Canvas.SetLeft(waitersButtons[i, j], j * waitersButtons[i, j].Width);
                        layerWaiter.Children.Add(waitersButtons[i, j]);
                        index++;
                    }
                }
            }
        }

        private void LoadTables()
        {
            var temp = context.C_Table.ToList();
            foreach (var item in temp)
            {
                Tables.Add(item);
            }

            TablesRows = (int)Math.Ceiling((double)Tables.Count / 2);
        }

        private void LoadWaiters()
        {
            var waiters = context.Staffs.Where(p => p.TypeStaff1.nameType == "Konobar").ToList();
            foreach (var item in waiters)
            {
                Waiters.Add(item);
            }
            WaitersRows = (int)Math.Ceiling((double)Waiters.Count / 2);
        }

        private void LoadReservations()
        {
            var reser = context.Reservations.ToList();
            foreach (var item in reser)
            {
                Reservations.Add(item);
            }
        }

        private void TableButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;

            if (clickedButton != null)
            {
                int tableId = (int)clickedButton.Tag;
                bool hasReservations = Reservations.Any(p => p.IdTable == tableId);

                if (hasReservations == true)
                {
                    MessageBox.Show("Sto je vec rezervisan");
                }
                else
                {
                    chosenTable = Tables.FirstOrDefault(table => table.IdTab == tableId);
                    if (chosenTable != null)
                    {
                        MessageBox.Show($"Odabrali ste sto {chosenTable.NameTab}");
                        isClickedTable = true;
                        btnReserved = clickedButton;
                    }
                }
            }
        }

        private void btnSumbit_Click(object sender, RoutedEventArgs e)
        {
            bool isExistingTelephone = context.People.Any(p => p.Telefon == tbTelefon.Text && p.PersonName != tbIme.Text);
            if (isClickedTable == false || isClickedWaiter == false || string.IsNullOrEmpty(tbIme.Text) || string.IsNullOrEmpty(tbTelefon.Text))
            {
                MessageBox.Show("Niste odabrali neku od opcija ili niste uneli podatke o kupcu.");
                return;
            }
            else if (tbIme.Text.Any(c => char.IsDigit(c)))
            {
                MessageBox.Show("Ime ne sme sadrzati brojeve.");
            }

            else if (isExistingTelephone)
            {
                MessageBox.Show($"Uneti broj telefona već postoji.");
            }
            else if (tbTelefon.Text.Any(char.IsLetter))
            {
                MessageBox.Show("Broj telefona ne sme sadržati slova.");
            }
            else
            {


                Person customer = new Person
                {
                    PersonName = tbIme.Text.Trim(),
                    Telefon = tbTelefon.Text.Trim()
                };

                context.People.Add(customer);
                context.SaveChanges();

                Guest guest = new Guest
                {
                    PersonID = customer.PersonID
                };

                context.Guests.Add(guest);
                context.SaveChanges();
                Reservation newReser = new Reservation
                {
                    IdTable = chosenTable.IdTab,
                    IdWaiter = chosenWaiter.idStaff,
                    IdCustomer = guest.GuestID,
                    Datum = DateTime.Now
                };

                context.Reservations.Add(newReser);
                context.SaveChanges();
                ObservableCollection<BillItem> temp = new ObservableCollection<BillItem>(BillItems);
                newBill = new Bill
                {
                    IdReservation = newReser.Id,
                    StatusBill = "neplacen",
                    BillItems = temp
                };
                context.Bills.Add(newBill);

                foreach (var item in BillItems)
                {
                    var article = context.Articles.FirstOrDefault(a => a.idArticle == item.Id);
                    if (article != null)
                    {
                        article.quantityArticle -= item.QuantityProduct;
                    }
                    if (article.quantityArticle == 0)
                    {
                        article.activityArticle = false;
                    }
                }

                BillItems.Clear();

                context.SaveChanges();


                LoadReservations();
                setTablesButtons();
                setWaitersButtons();

                MessageBox.Show("Rezervacija uspešno kreirana!");

                // Reset the form
                isClickedTable = false;
                isClickedWaiter = false;
                tbIme.Text = string.Empty;
                tbTelefon.Text = string.Empty;
                this.Close();
            }
        }


        private void WaiterButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;

            if (clickedButton != null)
            {
                int waiterId = (int)clickedButton.Tag;
                chosenWaiter = Waiters.FirstOrDefault(waiter => waiter.idStaff == waiterId);
                if (chosenWaiter != null)
                {
                    MessageBox.Show($"Odabrali ste konobara {chosenWaiter.Person.PersonName}");
                    isClickedWaiter = true;
                }
            }
        }

        private void btnFold_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
    
}
