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
    public class BookingReservationRepository : IBookingReservationRepository
    {
        private readonly IFuminiHotelManagementContext _dbContext;
        public BookingReservationRepository()
        {
            _dbContext = new FuminiHotelManagementContext();
        }
        public async Task CreateAsync(BookingReservation bookingReservation)
        {
            await _dbContext.BookingReservations.AddAsync(bookingReservation);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existingItem = await _dbContext.BookingReservations.FindAsync(id)
                ?? throw new Exception("Booking reservation not foumd");
            _dbContext.BookingReservations.Remove(existingItem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<BookingReservation>> GetAllAsync()
        {
            return await _dbContext.BookingReservations
                    .Include(b => b.Customer)
                    .Include(b => b.BookingDetails)
                        .ThenInclude(d => d.Room)
                            .ThenInclude(r => r.RoomType)
                            .ToListAsync();
        }

        public Task<Entities.BookingReservation?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Entities.BookingReservation bookingReservation)
        {
            var existing = await _dbContext.BookingReservations
        .FindAsync(bookingReservation.BookingReservationId);

            if (existing == null)
                throw new Exception("Booking not found");

            // Update từng thuộc tính
            if (bookingReservation.BookingDate.HasValue)
                existing.BookingDate = bookingReservation.BookingDate;

            if (bookingReservation.TotalPrice.HasValue)
                existing.TotalPrice = bookingReservation.TotalPrice;

            existing.CustomerId = bookingReservation.CustomerId;

            if (bookingReservation.BookingStatus.HasValue)
                existing.BookingStatus = bookingReservation.BookingStatus;

            // Lưu thay đổi
            await _dbContext.SaveChangesAsync();
        }
    }
}
