using BusinessLogicLayer.Abstraction;
using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
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

namespace LeHoangDuyWPF
{
    /// <summary>
    /// Interaction logic for CreateCustomerWindow.xaml
    /// </summary>
    public partial class CreateCustomerWindow : Window
    {
        private readonly ICustomerService _customerService;
        public CreateCustomerWindow()
        {
            InitializeComponent();
            _customerService = new CustomerService();
        }

        private async void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmailAddress.Text))
            {
                MessageBox.Show("Email is required.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtEmailAddress.Focus();
                return;
            }
            var customer = new Customer()
            {
                CustomerFullName = txtFullName.Text,
                Telephone = txtTelephone.Text,
                EmailAddress = txtEmailAddress.Text,
                CustomerBirthday = DatePickerBirthday.SelectedDate.HasValue
                ? DateOnly.FromDateTime(DatePickerBirthday.SelectedDate.Value)
                : null,
                CustomerStatus = byte.TryParse(txtStatus.Text, out byte status) ? status : null,
                Password = pbPassword.Password
            };
            await _customerService.CreateAsync(customer);
            DialogResult = true;
            this.Close();
        }
    }
}
