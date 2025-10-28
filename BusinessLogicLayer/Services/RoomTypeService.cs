using BusinessLogicLayer.Abstraction;
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
    public class RoomTypeService : IRoomTypeService
    {
        private readonly IRoomtypeRepository _roomTypeRepository;
        public RoomTypeService()
        {
            _roomTypeRepository = new RoomTypeRepository();
        }

        public async Task<IEnumerable<RoomType>> GetAllAsync()
        {
            return await _roomTypeRepository.GetAllAsync();
        }
    }
}
