using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Abstraction
{
    public interface IRoomInformationService
    {
        Task<IEnumerable<RoomInformation>> GetAllAsync();
        Task CreateAsync(RoomInformation room);
        Task UpdateAsync(RoomInformation room);
        Task DeleteAsync(int id);
    }
}
