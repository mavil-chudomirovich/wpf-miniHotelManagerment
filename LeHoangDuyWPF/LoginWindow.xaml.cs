using BusinessLogicLayer.Abstraction;
using BusinessLogicLayer.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
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
    /// Interaction logic for LoginWindonw.xaml
    /// </summary>
    public partial class LoginWindonw : Window
    {
        private readonly string _adminUserName;
        private readonly string _adminPassword;
        private readonly IConfiguration config;
        private readonly ICustomerService _customerService;
        public LoginWindonw()
        {
            InitializeComponent();
            config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
                   .AddJsonFile("appsettings.json", true, true)
                   .Build();
            _adminUserName = config["DefaultAdmin:Email"]!;
            _adminPassword = config["DefaultAdmin:Password"]!;
            _customerService = new CustomerService();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var email = txtEmail.Text;
            var password = txtPassword.Password;
            if (email == _adminUserName && password == _adminPassword)
            {
                MessageBox.Show("Login Success", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                var adminWindow = new AdminWindow();
                adminWindow.Show();
                this.Close();
            }else
            {
                var customer = await _customerService.LoginAsync(email, password);
                if (customer != null)
                {
                    MessageBox.Show("Login Success", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    var customerWindow = new CustomerWindow();
                    customerWindow.User = customer;
                    customerWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid email or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
