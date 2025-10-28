using DataAccessLayer.Abstraction;
using DataAccessLayer.AppDbContext;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DataAccessLayer.Repositories
{
    public class BookingDetailRepository : IBookingDetailRepository
    {
        private readonly IFuminiHotelManagementContext _dbContext;
        public BookingDetailRepository()
        {
            _dbContext = new FuminiHotelManagementContext();
        }

        public async Task CreateAsync(BookingDetail bookingDetail)
        {
            await _dbContext.BookingDetails.AddAsync(bookingDetail);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteByReservationIdAsync(int id)
        {
            var bookingDetails = await _dbContext.BookingDetails
                .Where(bd => bd.BookingReservationId == id).ToListAsync();
            _dbContext.BookingDetails.RemoveRange(bookingDetails);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<BookingDetail>> GetAllAsync()
        {
            return await _dbContext.BookingDetails
                .Include(bd => bd.BookingReservation)
                    .ThenInclude(br => br.Customer)
                .Include(br => br.Room)
                    .ThenInclude(br => br.RoomType)
                .ToListAsync();
        }

        public async Task<IEnumerable<BookingDetail>> GetByReservationIdAsync(int id)
        {
            return await _dbContext.BookingDetails
                .Include(br => br.Room)
                    .ThenInclude(br => br.RoomType)
                .Where(bd => bd.BookingReservationId == id)
                .ToListAsync();
        }

        public async Task<IEnumerable<BookingDetail>> GetByStartAndEndDate(DateOnly start, DateOnly end)
        {
            return await _dbContext.BookingDetails
                .Include(br => br.BookingReservation)
                    .ThenInclude(b => b.Customer)
                .Include (b => b.Room)
                    .ThenInclude (b => b.RoomType)
                .Where(b => b.StartDate >= start && b.EndDate <= end)
                .OrderByDescending(b => b.StartDate)
                .ToListAsync();
        }

        public async Task<bool> IsActiveBooking(int roomId)
        {
            var bookingDetails = await getByRoomId(roomId);
            foreach(var bookingDetail in bookingDetails)
            {
                if (bookingDetail.EndDate >= DateOnly.FromDateTime(DateTime.Now))
                {
                    return true;
                }
            }
            return false;
        }

        public async Task UpdateAsync(BookingDetail bookingDetail)
        {

            // tìm record cũ
            var existing = await _dbContext.BookingDetails.FindAsync(
                bookingDetail.BookingReservationId, bookingDetail.RoomId);

            if (existing == null)
                throw new Exception("BookingDetail not found");

            // Cập nhật giá trị
            existing.RoomId = bookingDetail.RoomId;        
            existing.StartDate = bookingDetail.StartDate;
            existing.EndDate = bookingDetail.EndDate;
            if (bookingDetail.ActualPrice.HasValue)
                existing.ActualPrice = bookingDetail.ActualPrice;
            await _dbContext.SaveChangesAsync();
        }

        private async Task<IEnumerable<BookingDetail>> getByRoomId(int roomId)
        {
            return await _dbContext.BookingDetails
                .Where(bd => bd.RoomId == roomId)
                .ToListAsync();
        }
    }
}
