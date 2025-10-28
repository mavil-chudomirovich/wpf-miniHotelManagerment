using BusinessLogicLayer.Abstraction;
using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using System.Windows;

namespace LeHoangDuyWPF
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        public Customer User { get; set; }
        private readonly ICustomerService _customerService;
        private readonly IBookingReservationService _reservationService;
        public CustomerWindow()
        {
            InitializeComponent();
            _customerService = new CustomerService();
            _reservationService = new BookingReservationService();
        }

        private async void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var customer = new Customer()
                {
                    CustomerId = int.Parse(txtId.Text),
                    CustomerFullName = txtFullName.Text,
                    Telephone = txtTelephone.Text,
                    EmailAddress = txtEmailAddress.Text,
                    CustomerBirthday = DateOnly.FromDateTime(DatePickerBirthday.SelectedDate ?? DateTime.Now.Date),
                    CustomerStatus = byte.TryParse(txtStatus.Text, out byte status) ? status : null,
                    Password = pbPassword.Password
                };
                await _customerService.UpdateAsync(customer);
                MessageBox.Show("Update successfully", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtId.Text = User.CustomerId.ToString();
            txtFullName.Text = User.CustomerFullName;
            txtTelephone.Text = User.Telephone;
            txtEmailAddress.Text = User.EmailAddress;
            txtStatus.Text = User.CustomerStatus.ToString();
            DatePickerBirthday.SelectedDate = User.CustomerBirthday.HasValue
            ? User.CustomerBirthday.Value.ToDateTime(TimeOnly.MinValue) // 00:00:00
            : (DateTime?)null;
            pbPassword.Password = User.Password;
            dgBooking.ItemsSource = await _reservationService.GetByCustomerId(User.CustomerId);

        }
    }
}
