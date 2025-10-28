using BusinessLogicLayer.Abstraction;
using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
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
using System.Windows.Shapes;

namespace LeHoangDuyWPF
{
    /// <summary>
    /// Interaction logic for UpdateCustomerWindow.xaml
    /// </summary>
    public partial class UpdateCustomerWindow : Window
    {
        private readonly ICustomerService _customerService;
        public Customer CustomerToUpdate { get; set; }
        public UpdateCustomerWindow()
        {
            InitializeComponent();
            _customerService = new CustomerService();
        }

        public UpdateCustomerWindow(ICustomerService customerService)
        {
            InitializeComponent();
            _customerService = customerService;
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
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtId.Text = CustomerToUpdate.CustomerId.ToString();
            txtFullName.Text = CustomerToUpdate.CustomerFullName;
            txtTelephone.Text = CustomerToUpdate.Telephone;
            txtEmailAddress.Text = CustomerToUpdate.EmailAddress;
            DatePickerBirthday.SelectedDate = DateTime.Parse(CustomerToUpdate.CustomerBirthday.ToString() ?? DateTime.Now.Date.ToString());
            txtStatus.Text = CustomerToUpdate.CustomerStatus.ToString();
            pbPassword.Password = CustomerToUpdate.Password;
        }
    }
}
