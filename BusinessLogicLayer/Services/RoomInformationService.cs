using BusinessLogicLayer.Abstraction;
using BusinessLogicLayer.Constant;
using DataAccessLayer.Abstraction;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class RoomInformationService : IRoomInformationService
    {
        private readonly IRoomInformationRepository _roomInfomationRepository;
        private readonly IRoomtypeRepository _roomtypeRepository;
        private readonly IBookingDetailRepository _bookingDetailRepository;
        public RoomInformationService()
        {
            _roomInfomationRepository = new RoomInformationRepository();
            _roomtypeRepository = new RoomTypeRepository();
            _bookingDetailRepository = new BookingDetailRepository();
        }

        public async Task CreateAsync(RoomInformation room)
        {
            var _roomType = (await _roomtypeRepository.GetByIdAsync(room.RoomTypeId)
                ?? throw new DirectoryNotFoundException("Room type not found"));
            await _roomInfomationRepository.CreateAsync(room);
        }

        public async Task DeleteAsync(int id)
        {
            var room = await _roomInfomationRepository.GetByIdAsync(id)
                ?? throw new DirectoryNotFoundException("Room not found");
            if(room.RoomStatus == (int)Constant.RoomStatus.Active)
            {
                if((await _bookingDetailRepository.IsActiveBooking(id)))
                {
                    room.RoomStatus = (int)Constant.RoomStatus.InActive;
                    await _roomInfomationRepository.UpdateAsync(room);
                }
                else
                {
                    await _roomInfomationRepository.DeleteAsync(room);
                }
            } 
        }

        public async Task<IEnumerable<RoomInformation>> GetAllAsync()
        {
           return (await _roomInfomationRepository.GetAllAsync()) ?? [];
        }

        public async Task UpdateAsync(RoomInformation room)
        {
           var roomType = (await _roomtypeRepository.GetByIdAsync(room.RoomTypeId))
                ?? throw new DirectoryNotFoundException("Room type not found");
            await _roomInfomationRepository.UpdateAsync(room);
        }
    }
}
