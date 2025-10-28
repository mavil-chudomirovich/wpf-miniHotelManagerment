using DataAccessLayer.Abstraction;
using DataAccessLayer.AppDbContext;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class RoomTypeRepository : IRoomtypeRepository
    {
        private readonly IFuminiHotelManagementContext _dbContext;
        public RoomTypeRepository()
        {
            _dbContext = new FuminiHotelManagementContext();
        }

        public async Task<RoomType?> GetByIdAsync(int id)
        {
            return await _dbContext.RoomTypes.FindAsync(id);
        }

        public async Task<IEnumerable<RoomType>> GetAllAsync()
        {
            return await _dbContext.RoomTypes.ToListAsync();
        }
    }
}
