using BusinessLogicLayer.Abstraction;
using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.WebSockets;
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

namespace LeHoangDuyWPF
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private readonly ICustomerService _customerService;
        private readonly IRoomInformationService _roomInfomationService;
        private readonly IBookingReservationService _bookingReservationService;
        private readonly IBookingDetailService _bookingDetailService;
        public AdminWindow()
        {
            InitializeComponent();
            _customerService = new CustomerService();
            _roomInfomationService = new RoomInformationService();
            _bookingReservationService = new BookingReservationService();
            _bookingDetailService = new BookingDetailService();
        }



        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                dgCustomer.ItemsSource = await _customerService.GetAllAsync();
                dgRoom.ItemsSource = await _roomInfomationService.GetAllAsync();
                dgBooking.ItemsSource = await _bookingReservationService.GetAllAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error when get data: {ex.Message}");
            }
        }

        private async void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            var popup = new CreateCustomerWindow();
            popup.Owner = this; // đặt MainWindow làm cha
            var result = popup.ShowDialog();
            if(result == true)
            {
                dgCustomer.ItemsSource = await _customerService.GetAllAsync();
                MessageBox.Show("Create Success", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        private async void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgCustomer.SelectedItem is Customer customer)
                {
                    var popup = new UpdateCustomerWindow(_customerService);
                    popup.CustomerToUpdate = customer;
                    popup.Owner = this;
                    
                    bool? result = popup.ShowDialog();

                    if (result == true)
                    {
                        dgCustomer.ItemsSource = await _customerService.GetAllAsync();
                        MessageBox.Show("Update Success", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgCustomer.SelectedItem is Customer customer)
                {
                    var result = MessageBox.Show(
                        "Do you want to delete this customer?",
                        "Warning",
                        MessageBoxButton.YesNo, MessageBoxImage.Question
                    );

                    if (result == MessageBoxResult.No) return;

                    await _customerService.DeleteAsync(customer.CustomerId);
                    dgCustomer.ItemsSource = await _customerService.GetAllAsync();
                    MessageBox.Show("Delete Success", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void BtnUpdateRoom_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgRoom.SelectedItem is RoomInformation room)
                {
                    var popup = new UpdateRoomWindow(_roomInfomationService);
                    popup.Owner = this;
                    popup.roomInformation = room;
                    bool? result = popup.ShowDialog();
                    if (result == true)
                    {
                        dgRoom.ItemsSource = await _roomInfomationService.GetAllAsync();
                        MessageBox.Show("Update Success", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void BtnDeleteRoom_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(dgRoom.SelectedItem is RoomInformation room)
                {
                    var result = MessageBox.Show(
                        "Do you want to delete this room?",
                        "Warning",
                        MessageBoxButton.YesNo, MessageBoxImage.Question
                    );

                    if (result == MessageBoxResult.No) return;

                    await _roomInfomationService.DeleteAsync(room.RoomId);
                    dgRoom.ItemsSource = await _roomInfomationService.GetAllAsync();
                    MessageBox.Show("Delete Success", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void BtnCreateRoom_Click(object sender, RoutedEventArgs e)
        {
            var popup = new CreateRoomWindow();
            popup.Owner = this; // đặt MainWindow làm cha
            var result = popup.ShowDialog();
            if(result == true)
            {
                dgRoom.ItemsSource = await _roomInfomationService.GetAllAsync();
                MessageBox.Show("Create Success", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private async void BtnCreateBooking_Click(object sender, RoutedEventArgs e)
        {
            var popup = new CreateBookingWindow();
            popup.Owner = this; // đặt MainWindow làm cha
            var result = popup.ShowDialog();
            if(result == true)
            {
                dgBooking.ItemsSource = await _bookingReservationService.GetAllAsync();
                MessageBox.Show("Create Success", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private async void BtnUpdateBooking_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(dgBooking.SelectedItem is BookingReservation bookingReservation)
                {
                    var popup = new UpdateBookingReservationWindow(_bookingReservationService);
                    popup.Owner = this;
                    popup.BookingReservation = bookingReservation;
                    bool? result = popup.ShowDialog();
                    if (result == true)
                    {
                        dgBooking.ItemsSource =  await _bookingReservationService.GetAllAsync();
                        MessageBox.Show("Update Success", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void BtnDeleteBooking_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgBooking.SelectedItem is BookingReservation booking)
                {
                    var result = MessageBox.Show(
                        "Do you want to delete this booking?",
                        "Warning",
                        MessageBoxButton.YesNo, MessageBoxImage.Question
                    );

                    if (result == MessageBoxResult.No) return;

                    await _bookingReservationService.DeleteAsync(booking.BookingReservationId);
                    dgBooking.ItemsSource = await _bookingReservationService.GetAllAsync();
                    MessageBox.Show("Delete Success", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void report_Click(object sender, RoutedEventArgs e)
        {
            if (DpStart.SelectedDate == null || DpEnd.SelectedDate == null)
            {
                MessageBox.Show("Please select both Start and End dates.");
                return;
            }
            var start = DateOnly.FromDateTime(DpStart.SelectedDate.Value);
            var end = DateOnly.FromDateTime(DpEnd.SelectedDate.Value);
            var data = await _bookingDetailService.GetByStartAndEndDate(start, end);
            dgReport.ItemsSource = data;
        }
    }
}
