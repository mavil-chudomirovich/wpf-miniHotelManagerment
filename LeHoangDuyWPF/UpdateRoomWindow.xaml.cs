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
    /// Interaction logic for UpdateRoomWindow.xaml
    /// </summary>
    public partial class UpdateRoomWindow : Window
    {
        private readonly IRoomInformationService _roomInformationService;
        private readonly IRoomTypeService _roomTypeService;
        public RoomInformation roomInformation;
        public UpdateRoomWindow()
        {
            InitializeComponent();
            _roomInformationService = new RoomInformationService();
            _roomTypeService = new RoomTypeService();
        }

        public UpdateRoomWindow(IRoomInformationService roomInformationService)
        {
            InitializeComponent();
            _roomInformationService = roomInformationService;
            _roomTypeService = new RoomTypeService();
        }

        private async void BtnUpdateRoom_Click(object sender, RoutedEventArgs e)
        {
            var roomInfomation = new RoomInformation()
            {
                RoomId = int.Parse(txtRoomId.Text),
                RoomNumber = txtRoomNumber.Text,
                RoomDetailDescription = txtRoomDetailDescription.Text,
                RoomMaxCapacity = int.TryParse(txtRoomMaxCapacity.Text, out int maxCapacity) ? maxCapacity : null,
                RoomTypeId = (int)cbxRoomType.SelectedValue,
                RoomStatus = byte.TryParse(txtRoomStatus.Text, out byte roomStatus) ? roomStatus : null,
                RoomPricePerDay = decimal.TryParse(txtRoomPricePerDay.Text, out decimal pricePerDay) ? pricePerDay : null,
            };
            await _roomInformationService.UpdateAsync(roomInfomation);
            DialogResult = true;
            Close();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbxRoomType.ItemsSource = await _roomTypeService.GetAllAsync();
            cbxRoomType.DisplayMemberPath = "RoomTypeName";
            cbxRoomType.SelectedValuePath = "RoomTypeId";
            txtRoomId.Text = roomInformation.RoomId.ToString();
            txtRoomNumber.Text = roomInformation.RoomNumber;
            txtRoomDetailDescription.Text = roomInformation.RoomDetailDescription;
            txtRoomMaxCapacity.Text = roomInformation.RoomMaxCapacity.ToString();
            cbxRoomType.SelectedValue = roomInformation.RoomTypeId;
            txtRoomStatus.Text = roomInformation.RoomStatus.ToString();
            txtRoomPricePerDay.Text = roomInformation.RoomPricePerDay.ToString();
        }
    }
}
