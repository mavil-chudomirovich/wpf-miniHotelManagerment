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
    public class RoomInformationRepository : IRoomInformationRepository
    {
        private readonly IFuminiHotelManagementContext _dbContext;
        public RoomInformationRepository()
        {
            _dbContext = new FuminiHotelManagementContext();
        }
        public async Task CreateAsync(RoomInformation room)
        {
            await _dbContext.RoomInformations.AddAsync(room);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(RoomInformation room)
        {
            _dbContext.RoomInformations.Remove(room);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<RoomInformation>> GetAllAsync()
        {
            return await _dbContext.RoomInformations
                .Include(r => r.RoomType)
                .ToListAsync();
        }

        public async Task<RoomInformation?> GetByIdAsync(int id)
        {
            return await _dbContext.RoomInformations.FindAsync(id);
        }

        public async Task UpdateAsync(RoomInformation room)
        {
            
            var existingRoom = _dbContext.RoomInformations.Find(room.RoomId);
            if (existingRoom != null)
            {
                if (room.RoomNumber is not null)
                    existingRoom.RoomNumber = room.RoomNumber;
                if(!string.IsNullOrEmpty(room.RoomDetailDescription))
                    existingRoom.RoomDetailDescription = room.RoomDetailDescription;
                if (room.RoomMaxCapacity != null) 
                    existingRoom.RoomMaxCapacity = room.RoomMaxCapacity;
                if (room.RoomTypeId != null)
                    existingRoom.RoomTypeId = room.RoomTypeId;
                if (room.RoomStatus != null)
                    existingRoom.RoomStatus = room.RoomStatus;
                if(room.RoomPricePerDay != null)
                    existingRoom.RoomPricePerDay = room.RoomPricePerDay;
                await _dbContext.SaveChangesAsync();
            }
            else throw new DirectoryNotFoundException("Room not found");

        }
    }
}
