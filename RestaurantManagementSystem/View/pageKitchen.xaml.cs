using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RestaurantManagementSystem.View
{
    public partial class pageKitchen : Page,INotifyPropertyChanged
    {
        RestaurantEntities context = new RestaurantEntities();

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        ObservableCollection<Bill> Bills { get; set; } = new ObservableCollection<Bill>();
        ObservableCollection<BillItem> BillItems { get; set; }= new ObservableCollection<BillItem>();
        public pageKitchen()
        {
            InitializeComponent();
            LoadBills();
            SetOrder();
        }
        
        private void LoadBills()
        {
            try
            {
               
                var bills = context.Bills.Include("Reservation.C_Table").Include("Reservation.Staff").ToList();

                
                foreach (var bill in bills)
                {
                    
                    Bills.Add(bill);
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greska prilikom ucitavanja racuna: {ex.Message}");
            }
        }


        private void SetOrder()
        {
            try
            {
                int addedCount = 0;
                for (int i = 0; i < Bills.Count; i++)
                {
                    if (Bills[i].StatusBill == "neplacen")
                    {
                        StackPanel sp1 = new StackPanel();
                        sp1.Width = 230;
                        sp1.Height = 450;
                        sp1.Orientation = Orientation.Vertical;
                        sp1.Margin = new Thickness(10);

                        StackPanel sp2 = new StackPanel();
                        sp2.Width = 230;
                        sp2.Height = 200;
                        sp2.Orientation = Orientation.Vertical;
                        sp2.Background = (Brush)this.FindResource("bgColor");

                        TextBlock billNo = new TextBlock();
                        billNo.Text = $"Račun: {Bills[i].Id}";
                        billNo.HorizontalAlignment = HorizontalAlignment.Center;
                        billNo.Margin = new Thickness(10);

                        TextBlock tableBill = new TextBlock();
                        if (Bills[i].Reservation?.C_Table != null)
                        {
                            tableBill.Text = $"Sto: {Bills[i].Reservation.C_Table.NameTab}";
                        }
                        else
                        {
                            tableBill.Text = "Sto: N/A";
                        }
                        tableBill.HorizontalAlignment = HorizontalAlignment.Center;
                        tableBill.Margin = new Thickness(10);

                        TextBlock waiterBill = new TextBlock();
                        if (Bills[i].Reservation?.Staff != null)
                        {
                            waiterBill.Text = $"Konobar: {Bills[i].Reservation.Staff.Person.PersonName}";
                        }
                        else
                        {
                            waiterBill.Text = "Konobar: N/A";
                        }
                        waiterBill.HorizontalAlignment = HorizontalAlignment.Center;
                        waiterBill.Margin = new Thickness(10);

                        TextBlock dateBill = new TextBlock();
                        if (Bills[i].Reservation != null)
                        {
                            dateBill.Text = $"{Bills[i].Reservation.Datum}";
                        }
                        else
                        {
                            dateBill.Text = "Datum: N/A";
                        }
                        dateBill.HorizontalAlignment = HorizontalAlignment.Center;
                        dateBill.Margin = new Thickness(10);

                        Button complete = new Button();
                        complete.Content = "Završi";
                        complete.HorizontalAlignment = HorizontalAlignment.Center;
                        complete.VerticalAlignment = VerticalAlignment.Bottom;
                        complete.Margin = new Thickness(10);
                        complete.FontSize = 15;
                        complete.Height = 50;
                        complete.Width = 100;
                        complete.Background = Brushes.Green;

                        
                        complete.Tag = Bills[i].Id;

                        complete.Click += CompleteButton_Click;

                        sp2.Children.Add(billNo);
                        sp2.Children.Add(tableBill);
                        sp2.Children.Add(waiterBill);
                        sp2.Children.Add(dateBill);

                        sp1.Children.Add(sp2);
                        
                        if (Bills[i].BillItems != null && Bills[i].BillItems.Count > 0)
                        {
                            for (int j = 0; j < Bills[i].BillItems.Count; j++)
                            {
                                TextBlock product = new TextBlock();
                                product.Text = $"{j + 1}. {Bills[i].BillItems[j].NameProduct} -- Količina: {Bills[i].BillItems[j].QuantityProduct}\n";
                                product.HorizontalAlignment = HorizontalAlignment.Left;
                                product.Margin = new Thickness(5);
                                sp1.Children.Add(product);
                            }
                        }
                        else
                        {
                            TextBlock product = new TextBlock();
                            product.Text = "Items: N/A";
                            product.HorizontalAlignment = HorizontalAlignment.Left;
                            product.Margin = new Thickness(5);
                            sp1.Children.Add(product);
                        }

                        sp1.Children.Add(complete);

                        //postavljam velicinu
                        double left = (i % 4) * 300;
                        double top = (i / 4) * 300;
                        Canvas.SetLeft(sp1, left);
                        Canvas.SetTop(sp1, top);

                        layer.Children.Add(sp1);
                        addedCount++;
                    }
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greska prilikom odredjivanja narudzbine: {ex.Message}");
            }
        }

        private void CompleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                int billId;
                if (int.TryParse(button.Tag.ToString(), out billId))
                {
                    Bill billToRemove = Bills.FirstOrDefault(b => b.Id == billId);
                    if (billToRemove != null)
                    {
                        try
                        {
                            billToRemove.StatusBill = "gotov";
                            context.SaveChanges();
                            MessageBox.Show("Narudžbina je uspešno završena.");

                            StackPanel parentStackPanel = button.Parent as StackPanel;
                            if (parentStackPanel != null)
                            {
                                Canvas parentCanvas = parentStackPanel.Parent as Canvas;
                                if (parentCanvas != null)
                                {
                                    parentCanvas.Children.Remove(parentStackPanel);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Greška prilikom brisanja računa iz baze podataka: {ex.Message}");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Račun nije pronađen.");
                    }
                }
                else
                {
                    MessageBox.Show("Nije moguće dobaviti ID računa.");
                }
            }
        }



    }
}
