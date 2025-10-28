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
    /// Interaction logic for CreateRoomWindow.xaml
    /// </summary>
    public partial class CreateRoomWindow : Window
    {
        private readonly IRoomInformationService _roomInformationService;
        private readonly IRoomTypeService _roomTypeService;
        public CreateRoomWindow()
        {
            InitializeComponent();
            _roomInformationService = new RoomInformationService();
                        _roomTypeService = new RoomTypeService();
        }

        private async void BtnCreateRoom_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRoomNumber.Text))
            {
                MessageBox.Show("RoomNumber is required.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtRoomNumber.Focus();
                return;
            }
            if (cbxRoomType.SelectedValue == null)
            {
                MessageBox.Show("Room Type is required.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                cbxRoomType.Focus();
                return;
            }
            var roomInfomation = new RoomInformation()
            {
                RoomNumber = txtRoomNumber.Text,
                RoomDetailDescription = txtRoomDetailDescription.Text,
                RoomMaxCapacity = int.TryParse(txtRoomMaxCapacity.Text, out int maxCapacity) ? maxCapacity : null,
                RoomTypeId = (int)cbxRoomType.SelectedValue,
                RoomStatus = byte.TryParse(txtRoomStatus.Text, out byte roomStatus) ? roomStatus : null,
                RoomPricePerDay = decimal.TryParse(txtRoomPricePerDay.Text, out decimal pricePerDay) ? pricePerDay : null,
            };
            await _roomInformationService.CreateAsync(roomInfomation);
            DialogResult = true;
            this.Close();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbxRoomType.ItemsSource = await _roomTypeService.GetAllAsync();
            cbxRoomType.DisplayMemberPath = "RoomTypeName";
            cbxRoomType.SelectedValuePath = "RoomTypeId";
        }
    }
}
