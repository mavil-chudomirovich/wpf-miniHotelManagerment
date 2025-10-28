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
    /// Interaction logic for UpdateBookingReservationWindow.xaml
    /// </summary>
    public partial class UpdateBookingReservationWindow : Window
    {
        public BookingReservation BookingReservation { get; set; }
        public IBookingReservationService _bookingReservationSerivce;
        private readonly ICustomerService _customerService;
        private readonly IRoomInformationService _roomInformationService;
        public UpdateBookingReservationWindow(IBookingReservationService bookingReservationService)
        {
            InitializeComponent();
            _customerService = new CustomerService();
            _roomInformationService = new RoomInformationService();
            _bookingReservationSerivce = bookingReservationService;
        }

        private async void BtnUpdateRoom_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var reservationId = int.Parse(txtReservationId.Text);
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
                if (CbStatus.SelectedItem == null)
                {
                    MessageBox.Show("Please select status", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                int status = int.Parse(((ComboBoxItem)CbStatus.SelectedItem).Tag.ToString()!);
                string startDateStr = DpStartDate.Text;
                string endDateStr = DpEndDate.Text;
                await _bookingReservationSerivce.UpdateAsync(reservationId, (int)customerId, selectedRooms, status, startDateStr, endDateStr);
                DialogResult = true;
                this.Close();

            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error when update booking: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtReservationId.Text = BookingReservation.BookingReservationId.ToString();
            CbCustomer.ItemsSource = await _customerService.GetAllAsync();
            CbCustomer.SelectedValue = BookingReservation.CustomerId;
            CbStatus.SelectedValue = BookingReservation.BookingStatus;
            LbRooms.ItemsSource = await _roomInformationService.GetAllAsync();
            var bookedRoomIds = BookingReservation.BookingDetails
                .Select(x => x.RoomId)
                .ToList();
            foreach (var item in LbRooms.Items.Cast<RoomInformation>())
            {
                if (bookedRoomIds.Contains(item.RoomId))
                    LbRooms.SelectedItems.Add(item);
            }
            DpStartDate.SelectedDate = BookingReservation.BookingDetails.FirstOrDefault()!.StartDate.ToDateTime(new TimeOnly(0, 0));
            DpEndDate.SelectedDate = BookingReservation.BookingDetails.FirstOrDefault()!.EndDate.ToDateTime(new TimeOnly(0, 0));
        }
    }
}
