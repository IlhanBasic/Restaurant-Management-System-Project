using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
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
using System.Windows.Threading;

namespace RestaurantManagementSystem.View
{
    /// <summary>
    /// Interaction logic for pagePOS.xaml
    /// </summary>
    public partial class pagePOS : Page, INotifyPropertyChanged
    {
        public bool PaidWithCard = false;
        public bool PaidWithCache = false;
        RestaurantEntities context = new RestaurantEntities();
        private ObservableCollection<Article> products;
        private ObservableCollection<BillItem> posBills;
        public ObservableCollection<Article> Products
        {
            get { return products; }
            set
            {
                products = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<BillItem> PosBills
        {
            get { return posBills; }
            set
            {
                posBills = value;
                OnPropertyChanged();
            }
        }


        

        public pagePOS()
        {
            InitializeComponent(); 

            Products = new ObservableCollection<Article>();
            PosBills = new ObservableCollection<BillItem>();
            var p = context.Articles.ToList();

            foreach (var item in p)
            {
                Products.Add(item);
            }
            InitTextBox();
        }
        
        
        private void InitTextBox()
        {
            tbTotalBill.Text = "Total: ";
            tbTotalBill.Text += PosBills.Sum(p => p.TotalPrice);
        }

        
        private bool _isFirstEdit = true;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

            if (_isFirstEdit)
            {
                _isFirstEdit = false;
                ((DataGrid)sender).IsReadOnly = true;
            }
        }
        
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            // Pretraga
            if (!string.IsNullOrEmpty(searchTerm))
            {
                var filteredProducts = Products.Where(pro => pro.nameArticle.ToLower().StartsWith(searchTerm.ToLower())).ToList();

                
                if (filteredProducts.Count > 0)
                {
                    Products.Clear();
                    foreach (var v in filteredProducts)
                    {
                        Products.Add(v);
                    }
                }
                else
                {
                    
                    MessageBox.Show("Nema rezultata za uneti pojam.", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                Products.Clear();
                var temp = context.Articles.ToList();
                foreach (var c in temp)
                {
                    Products.Add(c);
                }
            }
        }

        private void btnDecrease_Click(object sender, RoutedEventArgs e)
        {
            Button btnDecrease = sender as Button;
            if (btnDecrease != null)
            {
                StackPanel stackPanel = VisualTreeHelper.GetParent(btnDecrease) as StackPanel;
                if (stackPanel != null)
                {
                    TextBox tbQuantity = stackPanel.FindName("tbQuantity") as TextBox;
                    if (tbQuantity != null)
                    {
                        int quantity;
                        if (int.TryParse(tbQuantity.Text, out quantity))
                        {
                            if (quantity > 1)
                            {
                                quantity--;
                                tbQuantity.Text = quantity.ToString();
                            }
                        }
                    }
                }
            }
        }


        private void btnIncrease_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgProducts.SelectedItem as Article;
            if (selected != null)
            {
                Button btnIncrease = sender as Button;
                if (btnIncrease != null)
                {
                    StackPanel stackPanel = VisualTreeHelper.GetParent(btnIncrease) as StackPanel;
                    if (stackPanel != null)
                    {
                        TextBox tbQuantity = stackPanel.FindName("tbQuantity") as TextBox;
                        if (tbQuantity != null)
                        {
                            int quantity;
                            if (int.TryParse(tbQuantity.Text, out quantity))
                            {
                                if (quantity < selected.quantityArticle)
                                {
                                    quantity++;
                                    tbQuantity.Text = quantity.ToString();
                                }
                            }
                        }
                    }
                }
            }

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgProducts.SelectedItem as Article;
            if (selected != null)
            {
                Button btnAdd = sender as Button;
                if (btnAdd != null)
                {
                    StackPanel stackPanel = VisualTreeHelper.GetParent(btnAdd) as StackPanel;
                    if (stackPanel != null)
                    {
                        TextBox tbQuantity = stackPanel.FindName("tbQuantity") as TextBox;
                        if (tbQuantity != null)
                        {
                            int quantity;
                            if (int.TryParse(tbQuantity.Text, out quantity))
                            {
                                if (quantity >= 1 && quantity <= selected.quantityArticle)
                                {
                                    var existingItem = PosBills.FirstOrDefault(item => item.Id == selected.idArticle);
                                    if (existingItem != null)
                                    {
                                        if (existingItem.QuantityProduct + quantity <= selected.quantityArticle)
                                        {
                                            existingItem.QuantityProduct += quantity;
                                            existingItem.TotalPrice += quantity * selected.priceArticle;
                                            
                                        }
                                        else
                                        {
                                            MessageBox.Show($"Nije moguće dodati više od {selected.quantityArticle} proizvoda.");
                                        }
                                    }
                                    else
                                    {
                                        BillItem newItem = new BillItem
                                        {
                                            Id = selected.idArticle,
                                            NameProduct = selected.nameArticle,
                                            PriceProduct = selected.priceArticle,
                                            QuantityProduct = quantity,
                                            TotalPrice = quantity * selected.priceArticle
                                        };

                                        PosBills.Add(newItem);
                                    }
                                    InitTextBox();
                                    context.SaveChanges();
                                }
                                else
                                {
                                    MessageBox.Show($"Nije moguće dodati stavku računa.");
                                }
                            }
                        }
                    }
                }
            }
        }



        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgCart.SelectedItem as BillItem;
            if (selected != null)
            {
                MessageBoxResult result = MessageBox.Show("Da li ste sigurni da zelite da obrisete proizvod?", "Potvrda brisanja", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    var productToRemove = PosBills.FirstOrDefault(p => p.Id == selected.Id);
                    if (productToRemove != null)
                    {
                        
                        PosBills.Remove(productToRemove);
                        context.SaveChanges();
                        InitTextBox();
                    }
                }
                else
                {
                    MessageBox.Show("Proizvod nije obrisan.", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Morate prvo izabrati proizvod koji želite da obrišete.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnReservation_Click(object sender, RoutedEventArgs e)
        {
            if (PosBills.Count > 0)
            {
                newReservation nr = new newReservation(PosBills);
                nr.Closed += NewReservation_ReservationMade;
                nr.Show();
            }
            else
            {
                MessageBox.Show("Morate dodati proizvode prvo");
            }
        }


        private void NewReservation_ReservationMade(object sender, EventArgs e)
        {
            PosBills.Clear();
            tbTotalBill.Text = $"Total: {PosBills.Count}"; 
        }

        private void btnList_Click(object sender, RoutedEventArgs e)
        {
            ListBills lb = new ListBills();
            lb.Show();
        }

        
    }
}
