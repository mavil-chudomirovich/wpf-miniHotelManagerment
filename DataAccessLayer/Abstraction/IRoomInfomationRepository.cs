using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstraction
{
    public interface IRoomInfomationRepository
    {
        Task<IEnumerable<RoomInformation>> GetAllAsync();
        Task CreateAsync(RoomInformation room);
        Task UpdateAsync(RoomInformation room);
        Task<RoomInformation?> GetByIdAsync(int id);
        Task DeleteAsync(RoomInformation room);
    }
}
