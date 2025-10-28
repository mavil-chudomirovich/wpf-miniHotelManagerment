using BusinessLogicLayer.Abstraction;
using BusinessLogicLayer.Services;
using DataAccessLayer.Abstraction;
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
    /// Interaction logic for CreateBookingWindow.xaml
    /// </summary>
    public partial class CreateBookingWindow : Window
    {
        private readonly IBookingReservationService _bookingReservationService;
        private readonly IRoomInformationService _roomService;
        private readonly ICustomerService _customerService;
        public CreateBookingWindow()
        {
            InitializeComponent();
            _bookingReservationService = new BookingReservationService();
            _roomService = new RoomInformationService();
            _customerService = new CustomerService();

        }


        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CbCustomer.ItemsSource = await _customerService.GetAllAsync();
            LbRooms.ItemsSource = await _roomService.GetAllAsync();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var customerId = CbCustomer.SelectedValue;
                if (customerId == null)
                {
                    MessageBox.Show("Please select a customer", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var selectedRooms = LbRooms.SelectedItems
                                .Cast<RoomInformation>()
                                .ToList();
                if (selectedRooms.Count == 0)
                {
                    MessageBox.Show("Please select at least one room", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if(CbStatus.SelectedItem == null)
                {
                    MessageBox.Show("Please select status", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                int status = int.Parse(((ComboBoxItem)CbStatus.SelectedItem).Tag.ToString()!);
                string startDateStr = DpStartDate.Text;
                string endDateStr = DpEndDate.Text;
                await _bookingReservationService.CreateBookingAsync(
                    (int)customerId,
                    selectedRooms,
                    status,
                    startDateStr,
                    endDateStr
                );
                DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error when create booking: {ex.Message}", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }
    }
}
