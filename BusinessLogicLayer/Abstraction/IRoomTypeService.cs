using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Abstraction
{
    public interface IRoomTypeService
    {
        Task<IEnumerable<RoomType>> GetAllAsync();
    }
}
